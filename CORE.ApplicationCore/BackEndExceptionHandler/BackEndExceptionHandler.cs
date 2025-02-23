﻿using Core.ApplicationCore.BackEndExceptionHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCore.BackEndExceptionHandler
{
    public class BackEndExceptionHandler : IBackEndExceptionHandler
    {
        public void ExceptionOperations<TException>(string additionalMessage, TException ex) where TException : Exception
        {
            string finalExceptionMessage = FillExceptionMessage(additionalMessage, ex);

            System.Diagnostics.Debug.WriteLine($"\n+++++ {finalExceptionMessage}");
        }

        private static string FillExceptionMessage<T>(string additionalMessage, T exception) where T : Exception
        {
            const string exceptionMessage = "ERROR - {0}!\n" +
                                            "Exception Message: {1}\n" +
                                            "Inner Exception Message: {2}\n" +
                                            "Stack Trace: {3}";

            return string.Format(exceptionMessage, additionalMessage,
                exception.Message,
                exception.InnerException?.Message,
                exception.StackTrace);
        }

    }
}
