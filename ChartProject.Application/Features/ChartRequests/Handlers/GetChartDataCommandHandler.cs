using CharProject.Application.Features.ChartRequests.Commands;
using ChartProject.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CharProject.Application.Features.ChartRequests.Handlers
{
    public class GetChartDataCommandHandler : IRequestHandler<GetChartDataCommand, GetChartDataResponse>
    {
        private readonly IChartDataService _chartDataService;

        public GetChartDataCommandHandler(IChartDataService chartDataService)
        {
            _chartDataService = chartDataService;
        }

        public async Task<GetChartDataResponse> Handle(GetChartDataCommand request, CancellationToken cancellationToken)
        {
            var data = await _chartDataService.GetChartData(request.ChartRequest);
            return new GetChartDataResponse
            {
                Labels = data.Labels,
                Data = data.Data
            };
        }
    }
}
