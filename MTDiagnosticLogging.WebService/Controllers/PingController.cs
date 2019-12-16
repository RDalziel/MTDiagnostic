namespace MTDiagnosticLogging.WebService.Controllers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Messages;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        public PingController(IBus bus)
        {
            _bus = bus;
        }

        private readonly IBus _bus;

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Ping()
        {
            await _bus.Publish(new Ping());
            return Ok();
        }
    }
}