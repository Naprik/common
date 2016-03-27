using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beehive.Std.Concurrent.Executors;

namespace Beehive.Std
{
    public static class ExecutionContexts
    {
        public static Executor Global => null;

        public static Executor Current => new SynchronousExecutor();
    }
}
