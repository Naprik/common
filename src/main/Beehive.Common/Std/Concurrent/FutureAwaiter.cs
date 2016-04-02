using System;
using System.Runtime.CompilerServices;

namespace Beehive.Common.Std.Concurrent
{
    public class FutureAwaiter<TResult> : INotifyCompletion
    {
        public bool IsCompleted { get; } = false;

        public Try<TResult> GetResult() => null;

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }
}
