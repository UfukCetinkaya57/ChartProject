using CharProject.Application.Features.ChartRequests.Queries;
using ChartProject.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CharProject.Application.Features.ChartRequests.Handlers
{
    public class GetProceduresViewsAndFunctionsQueryHandler : IRequestHandler<GetProceduresViewsAndFunctionsQuery, GetProceduresViewsAndFunctionsResponse>
    {
        private readonly IChartDataService _chartDataService;

        public GetProceduresViewsAndFunctionsQueryHandler(IChartDataService chartDataService)
        {
            _chartDataService = chartDataService;
        }

        public async Task<GetProceduresViewsAndFunctionsResponse> Handle(GetProceduresViewsAndFunctionsQuery request, CancellationToken cancellationToken)
        {
            return await _chartDataService.GetProceduresViewsAndFunctions(request.DbConnection);
        }
    }
}
