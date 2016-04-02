using System;
using Beehive.Common.Std.Concurrent;

namespace Beehive.Common.Std
{
    public static class Future
    {
        public static Future<TVal> Value<TVal>(TVal value) => null;

        public static Future<TVal> Delay<TVal>(Func<TVal> expr) => null;
    }

    public abstract class Future<TVal>
    {
        internal abstract TVal Value { get; }

        public abstract Future<TOut> FlatMap<TOut>(Func<TVal, Future<TOut>> expr);
    }

    public static class FutureOps
    {
        public static Future<TOut> Map<TVal, TOut>(this Future<TVal> ctx, Func<TVal, TOut> expr)
            => ctx.FlatMap(_ => Future.Delay(() => expr(_)));

        public static Future<TOut> Select<T, TOut>(this Future<T> ctx, Func<T, TOut> expr)
            => ctx.Map(expr);

        public static Future<TOut> SelectMany<TIn, TOut>(this Future<TIn> ctx, Func<TIn, Future<TOut>> func)
            => ctx.FlatMap(func);

        public static Future<TR> SelectMany<T, TTR, TR>(this Future<T> a, Func<T, Future<TTR>> fn, Func<T, TTR, TR> cp)
            => a.SelectMany(x => fn(x).SelectMany(y => Future.Delay(() => cp(x, y))));

        public static FutureAwaiter<TVal> GetAwaiter<TVal>(this Future<TVal> ctx) 
            => new FutureAwaiter<TVal>();

        public static Try<TVal> Run<TVal>(this Future<TVal> ctx) 
            => ctx.RunAt(ExecutionContexts.Current);

        public static Try<TVal> RunAt<TVal>(this Future<TVal> future, Executor context)
            => null;
    }
}
