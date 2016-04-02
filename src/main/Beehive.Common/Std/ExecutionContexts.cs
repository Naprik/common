using Beehive.Common.Std.Concurrent.Executors;

namespace Beehive.Common.Std
{
    public static class ExecutionContexts
    {
        public static Executor Global => null;

        public static Executor Current => new SynchronousExecutor();
    }
}
