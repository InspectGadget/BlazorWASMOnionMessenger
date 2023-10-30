
namespace BlazorWASMOnionMessenger.Application.Common.Exceptions
{
    public class CustomAuthenticationException : Exception
    {
        public CustomAuthenticationException(string message) : base(message) { }

    }
}
