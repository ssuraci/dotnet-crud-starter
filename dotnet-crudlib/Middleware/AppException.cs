using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebapiTemplate.WebapiException
{
    [Serializable]
    public class AppException : Exception
    {

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
