using System.Threading.Tasks;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IApiClient
{
    Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request);
}
