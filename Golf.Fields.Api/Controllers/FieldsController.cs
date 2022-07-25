using Golf.Fields.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golf.Fields.Api
{

    [ApiController]
    [ApiVersion(ApiConstants.VERSION)]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [Authorize]
    public class FieldsController : ControllerBase
    {


        private readonly AutoMapper.IMapper _mapper;

        public FieldsController(AutoMapper.IMapper mapper)
        {
            _mapper = mapper;
        }



        [HttpGet("[action]")]
        public async Task<IActionResult> Find(int skip = 0, int limit = 0, string? country = null, string? city = null, string? orderBy = null)
        {
            try
            {
                var svc = new FieldServices();

                var rawResult = await svc.Find(skip, limit, country, city, orderBy);

                var apiResult = _mapper.Map<SearchResultModel<FieldModel>>(rawResult);

                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
    }
}

