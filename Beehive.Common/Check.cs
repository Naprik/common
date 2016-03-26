using System;
using System.Diagnostics;
using Beehive.Properties;

namespace Beehive
{
    /// <summary>
    /// This class is made up of utility methods aimed to perform parameters check
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        /// <summary>
        /// Checks if the object is not null and throws an appropriate exception otherwise
        /// </summary>
        /// <typeparam name="TObj">Must be a reference type</typeparam>
        /// <param name="obj">Object to check</param>
        /// <param name="parameterName">Name of parameter</param>
        /// <param name="exceptionMessage">Message for the exception. In case of absence, it will be replaced by the default one</param>
        public static void ForNullReference<TObj>(TObj obj, string parameterName = null, string exceptionMessage = null)
        {
            if (null == obj) throw new ArgumentNullException(parameterName, exceptionMessage ?? Resources.Exc_NullArgument);
        }

        /// <summary>
        /// Checks if the string is not null, empty or whitespace and throws an exception otherwise
        /// </summary>
        /// <param name="obj">Parameter for check</param>
        /// <param name="parameterName">Name of parameter</param>
        /// <param name="exceptionMessage">Message for the exception. In case of absence, it will be replaced by the default one</param>
        public static void ForEmptyString(string obj, string parameterName = null, string exceptionMessage = null)
        {
            if (String.IsNullOrWhiteSpace(obj)) throw new ArgumentNullException(parameterName, exceptionMessage ?? Resources.Exc_NullArgument);
        }
    }
}
