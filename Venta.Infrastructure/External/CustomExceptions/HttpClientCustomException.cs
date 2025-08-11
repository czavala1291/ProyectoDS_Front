
namespace Venta.Infrastructure.External.CustomExceptions
{
    public class HttpClientCustomException : Exception
    {
        public int HttpCodeResponse { get; }
        public string BodyResponse { get; }

        public HttpClientCustomException(int httpCodeResponse, string bodyResponse)
            : base($"HTTP Error {httpCodeResponse}: {bodyResponse}")
        {
            HttpCodeResponse = httpCodeResponse;
            BodyResponse = bodyResponse;
        }

        public HttpClientCustomException(int httpCodeResponse, string bodyResponse, Exception innerException)
            : base($"HTTP Error {httpCodeResponse}: {bodyResponse}", innerException)
        {
            HttpCodeResponse = httpCodeResponse;
            BodyResponse = bodyResponse;
        }
    }
}
