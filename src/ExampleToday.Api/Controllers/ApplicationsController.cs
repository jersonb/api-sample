using ExampleToday.Api.Models;
using ExampleToday.Api.ViewObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ExampleToday.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = "Application")]
public class ApplicationsController : ControllerBase
{
    private readonly List<Phrase> _phrases;

    public ApplicationsController(FamousPhrases famousPhrases)
    {
        _phrases = famousPhrases.Data;
    }

    [HttpGet("all-famous-phrases")]
    [SwaggerOperation("Obtem todas as frases (sem filtro). Apenas para outras aplicações (requer autorização específica).")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<IEnumerable<Phrase>>))]
    public IActionResult GetAll()
    {
        return Ok(new SuccessResponse<IEnumerable<Phrase>>(_phrases));
    }
}