using CharProject.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace CharProject.Application.Features.ChartRequests.Commands
{
    public class GetChartDataCommand : IRequest<GetChartDataResponse>
    {
        public ChartRequest? ChartRequest { get; set; }

    }


    public class GetChartDataResponse
    {
        public List<string> Labels { get; set; }
        public List<decimal> Data { get; set; }
    }
}
