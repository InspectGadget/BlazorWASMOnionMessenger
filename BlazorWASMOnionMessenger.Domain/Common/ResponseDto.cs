namespace BlazorWASMOnionMessenger.Domain.Common
{
    public class ResponseDto
    {
        public bool IsSuccessful { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
