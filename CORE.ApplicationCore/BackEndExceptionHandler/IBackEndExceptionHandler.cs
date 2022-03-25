using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCore.BackEndExceptionHandler
{
    public interface IBackEndExceptionHandler
    {
        void ExceptionOperations<TException>(string additionalMessage, TException ex) where TException : Exception;
    }
}
