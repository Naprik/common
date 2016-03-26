using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Beehive.Properties;

namespace Beehive.Std
{
    using static Maybe;
    
    public static class Maybe
    {
        /// <summary>
        /// Creates a default [None] projection of Maybe effect
        /// </summary>
        public static Maybe<TVal> NoneFor<TVal>() => new None<TVal>();

        /// <summary>
        /// Creates a non-empty [Some] projection of Maybe effect
        /// </summary>
        /// <param name="value">Value must not be a null reference - an exception will thrown otherwise</param>
        public static Maybe<TVal> SomeFor<TVal>(TVal value)
        {
            Check.ForNullReference(value, "Unable to wrap null reference into Maybe effect");
            return new Some<TVal>(value);
        } 

        /// <summary>
        /// Applies a monadic effect Maybe to the call-by-name parameter specified
        /// </summary>
        public static Maybe<TVal> ApplyTo<TVal>(Func<TVal> expr)
        {
            var value = expr();
            return value != null ? SomeFor(value) : NoneFor<TVal>();
        }

        /// <summary>
        /// Applies a monadic effect Maybe to the parameter specified
        /// </summary>
        public static Maybe<TVal> ApplyTo<TVal>(TVal value) => ApplyTo(() => value);
    }

    /// <summary>
    /// Monadic effect Maybe that implies a possibility of absence of the value
    /// </summary>
    public abstract class Maybe<TIn>
    {
        internal abstract bool IsEmpty { get; }

        public abstract Maybe<TOut> FlatMap<TOut>(Func<TIn, Maybe<TOut>> func);
    }

    class Some<TVal> : Maybe<TVal>
    {
        public TVal Value { get; }

        internal override bool IsEmpty { get; } = false;

        public Some(TVal value)
        {
            Value = value;
        }

        public override Maybe<TOut> FlatMap<TOut>(Func<TVal, Maybe<TOut>> func) => func(Value);
    }

    class None<TVal> : Maybe<TVal>
    {
        internal override bool IsEmpty { get; } = true;

        public override Maybe<TOut> FlatMap<TOut>(Func<TVal, Maybe<TOut>> func) => NoneFor<TOut>();
    }

    public static class MaybeExtensions
    {
        public static bool HasValue<TVal>(this Maybe<TVal> ctx) => !ctx.IsEmpty;

        public static bool IsEmpty<TVal>(this Maybe<TVal> ctx) => ctx.IsEmpty;

        public static Maybe<TOut> Map<TVal, TOut>(this Maybe<TVal> ctx, Func<TVal, TOut> map) 
            => ctx.FlatMap(_ => ApplyTo(map(_)));

        public static TValue GetValue<TValue>(this Maybe<TValue> ctx) => GetValueOr(ctx, () =>
        {
            throw new NoneException(Resources.Exc_NoneExtraction);
        });

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static TValue GetValueOr<TValue>(this Maybe<TValue> ctx, Func<TValue> fallback) 
            => ctx.IsEmpty ? fallback() : (ctx as Some<TValue>).Value;

        public static TValue GetValueOr<TValue>(this Maybe<TValue> ctx, TValue fallback)
            => GetValueOr(ctx, () => fallback);

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> ctx, Func<T, TResult> func)
            => ctx.Map(func);

        public static Maybe<TOut> SelectMany<TIn, TOut>(this Maybe<TIn> ctx, Func<TIn, Maybe<TOut>> func)
            => ctx.FlatMap(func);

        public static Maybe<TR> SelectMany<T, TTR, TR>(this Maybe<T> a, Func<T, Maybe<TTR>> fn, Func<T, TTR, TR> cp) 
            => a.SelectMany(x => fn(x).SelectMany(y => cp(x, y).AsMaybe()));

        public static Maybe<T> AsMaybe<T>(this T obj) => ApplyTo(obj);
    }
    
    public class NoneException : Exception
    {
        public NoneException(string message = "", Exception inner = null) : base(message, inner) { }
    }
}
