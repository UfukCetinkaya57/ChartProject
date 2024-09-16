using CharProject.Application.Features.ChartRequests.Commands;
using CharProject.Application.Features.ChartRequests.Queries;
using CharProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ChartProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChartRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getProcedureViewsAndFunctions")]
        public async Task<IActionResult> GetProceduresViewsAndFunctions([FromQuery] string dbConnection)
        {
            var query = new GetProceduresViewsAndFunctionsQuery { DbConnection = dbConnection };
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpPost("getchartdata")]
        public async Task<IActionResult> GetChartData([FromBody] GetChartDataCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }



    }
}
