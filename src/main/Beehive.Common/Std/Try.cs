using System;
using System.Diagnostics.CodeAnalysis;

namespace Beehive.Common.Std
{
    public static class Try
    {
        public static Try<TVal> Failure<TVal>(Exception cause = null) => new Failure<TVal>(cause ?? new Exception("Default cause for failure"));

        public static Try<TVal> Success<TVal>(TVal value) => new Success<TVal>(value);
    }

    public abstract class Try<TValue>
    {
        internal abstract bool IsFailure { get; }

        public abstract Try<TOut> FlatMap<TOut>(Func<TValue, Try<TOut>> expr);
    }

    class Success<TValue> : Try<TValue>
    {
        public TValue Value { get; }

        internal override bool IsFailure { get; } = false;
        
        public Success(TValue value)
        {
            Value = value;
        }

        public override Try<TOut> FlatMap<TOut>(Func<TValue, Try<TOut>> expr) => expr(Value);
    }

    class Failure<TValue> : Try<TValue>
    {
        public Exception Cause { get; }

        internal override bool IsFailure { get; } = true;

        public Failure(Exception cause)
        {
            Cause = cause;
        }

        public override Try<TOut> FlatMap<TOut>(Func<TValue, Try<TOut>> expr) => Try.Failure<TOut>(Cause);
    }

    public static class TryOps
    {
        public static Try<TVal> Try<TVal>(Func<TVal> expr)
        {
            try
            {
                return new Success<TVal>(expr());
            }
            catch (Exception e)
            {
                return new Failure<TVal>(e);
            }
        }

        public static Try<TOut> Map<TVal, TOut>(this Try<TVal> ctx, Func<TVal, TOut> expr)
            => ctx.FlatMap(_ => Try(() => expr(_)));

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static TVal GetValueOr<TVal>(this Try<TVal> ctx, Func<TVal> fallback) 
            => ctx.IsFailure ? fallback() : (ctx as Success<TVal>).Value;

        public static TVal GetValue<TVal>(this Try<TVal> ctx) => ctx.GetValueOr(() =>
        {
            throw new FailureException("Unable to extract value from [Failure] projection");
        });

        public static Try<TResult> Select<T, TResult>(this Try<T> ctx, Func<T, TResult> func)
            => ctx.Map(func);

        public static Try<TOut> SelectMany<TIn, TOut>(this Try<TIn> ctx, Func<TIn, Try<TOut>> func)
            => ctx.FlatMap(func);

        public static Try<TR> SelectMany<T, TTR, TR>(this Try<T> a, Func<T, Try<TTR>> fn, Func<T, TTR, TR> cp) 
            => a.SelectMany(x => fn(x).SelectMany(y => cp(x, y).AsTry()));

        public static Try<TVal> AsTry<TVal>(this TVal ctx) 
            => new Success<TVal>(ctx);

        public static Try<TVal> AsTry<TVal>(this Func<TVal> ctx) 
            => Try(ctx);
    }

    [Serializable]
    public class FailureException : Exception
    {
        public FailureException(string message = null, Exception inner = null) : base(message, inner) { }
    }
}
