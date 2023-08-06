using Microsoft.AspNetCore.Mvc;

namespace ExampleToday.Api.Controllers
{
    [ApiController]
    [Route("phrases")]
    public class PhrasesController : ControllerBase
    {
        [HttpGet]
        public string Get() 
        {
            return "Ok!";
        }
    }
}