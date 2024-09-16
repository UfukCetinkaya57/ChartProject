using CharProject.Application.Features.ChartRequests.Commands;
using CharProject.Application.Features.ChartRequests.Queries;
using CharProject.Domain.Entities;
using System.Threading.Tasks;

namespace ChartProject.Application.Services
{
    public interface IChartDataService
    {
        Task<GetChartDataResponse> GetChartData(ChartRequest request);
        Task<GetProceduresViewsAndFunctionsResponse> GetProceduresViewsAndFunctions(string dbConnection);
    }
}
