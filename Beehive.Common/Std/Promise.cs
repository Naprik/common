using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive.Std
{
    public class Promise<TVal>
    {
        Try<TVal> _cont;

        public Future<TVal> Future { get; }

        public void Success(TVal result)
        {
            _cont = Try.Success(result);
        }

        public void Failure(Exception cause)
        {
            _cont = Try.Failure<TVal>(cause);
        }
    }
}
