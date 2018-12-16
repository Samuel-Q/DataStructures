using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresTests
{
    /// <summary>
    /// The exception to throw when a test case is incorrect.
    /// </summary>
    public class InvalidTestCaseException : Exception
    {
        public InvalidTestCaseException()
        {
        }

        public InvalidTestCaseException(string message) : base(message)
        {
        }

        public InvalidTestCaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
