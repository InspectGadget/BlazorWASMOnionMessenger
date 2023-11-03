
namespace BlazorWASMOnionMessenger.Application.Common.Exceptions
{
    public class CustomAuthenticationException : Exception
    {
        public CustomAuthenticationException() 
        {
        }

        public CustomAuthenticationException(string message) : base(message) 
        {
        }

        public CustomAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
