using System;

namespace mForex.API.Exceptions
{
    
    [Serializable]
    public class AuthenticationException : Exception
    {
        public LoginStatus LoginStatus { get; private set; }
        
        public AuthenticationException() { }
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, LoginStatus loginStatus) : base(message) { LoginStatus = loginStatus;  }
        public AuthenticationException(string message, Exception inner) : base(message, inner) { }
        protected AuthenticationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
