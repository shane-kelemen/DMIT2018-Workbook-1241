﻿namespace HogWildWebApp.HelperClasses
{
    public static class BlazorHelperClass
    {
        //	Gets the Exception instance that caused the current exception.
        //	An object that describes the error that caused the current exception.
        public static Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
    }
}
