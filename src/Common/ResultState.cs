using System;

namespace Common
{
    public class ResultState
    {
        public ResultState(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public ResultState(bool isSuccessful, string message, Exception thrownExeption)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            ThrownException = thrownExeption;
        }

        public bool IsSuccessful { get; set; }

        public Exception ThrownException { get; set; }

        public string Message { get; set; }

    }
}
