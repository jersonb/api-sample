using ExampleToday.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleToday.Api.Controllers
{
    [ApiController]
    [Route("phrases")]
    public class PhrasesController : ControllerBase
    {
        private static readonly List<Phrase> Phrases = new()
        {
            new ()
            {
                Id = 1,
                Author = "Fulano",
                Content = "Foo"
            },
            new ()
            {
                Id = 2,
                Author = "Beltrano",
                Content = "Bar"
            },new ()
            {
                Id = 3,
                Author = "Ciclano",
                Content = "Fizz"
            },
        };

        [HttpGet]
        public List<Phrase> Get()
        {
            return Phrases;
        }

        [HttpPost]
        public void Post(Phrase phrase)
        {
            Phrases.Add(phrase);
        }
    }
}