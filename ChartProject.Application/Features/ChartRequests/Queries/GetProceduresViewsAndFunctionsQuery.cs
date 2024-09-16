using MediatR;
using System.Collections.Generic;

namespace CharProject.Application.Features.ChartRequests.Queries
{
    public class GetProceduresViewsAndFunctionsQuery : IRequest<GetProceduresViewsAndFunctionsResponse>
    {
        public string DbConnection { get; set; }
    }

    public class GetProceduresViewsAndFunctionsResponse
    {
        public List<string> Procedures { get; set; }
        public List<string> Views { get; set; }
        public List<string> Functions { get; set; }
    }
}
