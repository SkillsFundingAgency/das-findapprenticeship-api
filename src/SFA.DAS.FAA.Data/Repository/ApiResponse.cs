using System.Net;

namespace SFA.DAS.FAA.Data.Repository;
public class ApiResponse<TResponse>
{
    public TResponse Body { get; }
    public HttpStatusCode StatusCode { get; }
    public string ErrorContent { get; }

    public ApiResponse(TResponse body, HttpStatusCode statusCode, string errorContent)
    {
        Body = body;
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }
}
