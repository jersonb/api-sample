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

        [HttpGet("{id}")]
        public Phrase? Get(int id)
        {
            return Phrases.Find(p => p.Id == id);
        }

        [HttpPost]
        public void Post(Phrase phrase)
        {
            Phrases.Add(phrase);
        }

        [HttpPut("{id}")]
        public void Put(int id, Phrase phrase)
        {
            phrase.Id = id;
            var findedPhrase = Phrases.Find(p => p.Id == id);

            if (findedPhrase != null)
            {
                findedPhrase.Author = phrase.Author;
                findedPhrase.Content = phrase.Content;
            }
        }

        [HttpDelete]
        public void Delete(int id, Phrase phrase)
        {
            if (phrase.Id == id)
            {
                var i = Phrases.FindIndex(p => p.Id == id);
                Phrases.RemoveAt(i);
            }
        }

    }
}