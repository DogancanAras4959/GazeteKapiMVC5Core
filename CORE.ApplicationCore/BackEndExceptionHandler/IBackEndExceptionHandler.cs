﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ApplicationCore.BackEndExceptionHandler
{
    public interface IBackEndExceptionHandler
    {
        void ExceptionOperations<TException>(string additionalMessage, TException ex) where TException : Exception;
    }
}
