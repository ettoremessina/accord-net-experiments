using System;

namespace Regressors.Exceptions
{
    public class NotTrainedException : Exception
    {
        public NotTrainedException() : 
            this("Not trained exception", null)
        {

        }

        public NotTrainedException(string message) : 
            this(message, null)
        {

        }

        public NotTrainedException(string message, Exception innerException) :
            base(message, innerException)
        {
            
        }
    }
}