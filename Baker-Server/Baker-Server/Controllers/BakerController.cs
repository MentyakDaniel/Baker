using Baker_Server.Database;
using Baker_Server.Database.Entities;
using Baker_Server.Services;
using Baker_Server.Services.Dto;
using Baker_Server.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Baker_Server.Controllers
{
    [ApiController]
    [Route("baker")]
    public class BakerController : ControllerBase
    {
        private readonly ILogger<BakerController> _logger;
        private readonly IBakerService _service;
        private readonly AppDbContext _context;

        public BakerController(ILogger<BakerController> logger, IBakerService service, AppDbContext context)
        {
            _logger = logger;
            _service = service;
            _context = context;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<BunSaleDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<ActionResult<ICollection<BunSaleDto>>> GetAlLSales()
        {
            try
            {
                ICollection<BunSaleDto> result = await _service.GetAllSales();

                return Ok(result);
            }
            catch(Exception exp)
            {
                await _context.Log(exp.Message);

                return BadRequest(exp.Message);
            }
        }

        [HttpPost("bake")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<ActionResult<bool>> Bake([FromBody]int count)
        {
            try
            {
                await _service.StartBake(count);

                return Ok(true);
            }
            catch (Exception exp)
            {
                await _context.Log(exp.Message);

                return BadRequest(exp.Message);
            }
        }

        [HttpGet("bun-names")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<EnumValueDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public ActionResult<ICollection<EnumValueDto>> GetBunNames()
        {
            try
            {
                ICollection<EnumValueDto> result = BakerServiceExtension
                    .GetEnumValues(typeof(BunNameEnum));

                return Ok(result);
            }
            catch (Exception exp)
            {
                 _context.Log(exp.Message).Wait();

                return BadRequest(exp.Message);
            }
        }
    }
}
