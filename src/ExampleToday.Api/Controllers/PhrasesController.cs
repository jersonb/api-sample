using System.Globalization;
using ExampleToday.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleToday.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PhrasesController : ControllerBase
    {
        private static readonly List<Phrase> Phrases = DataMock.Data;

        [HttpGet]
        public IActionResult Get(string? author, string? term, int page = 1, int limit = 10)
        {
            var phrases = Phrases.AsEnumerable();

            if (!string.IsNullOrEmpty(author))
            {
                phrases = phrases.Where(p => p.Author.ContainsInsensitive(author));
            }

            if (!string.IsNullOrEmpty(term))
            {
                phrases = phrases.Where(p => p.Content.ContainsInsensitive(term));
            }

            page = (page * limit) - limit;

            var result = phrases.Skip(page).Take(limit);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = Phrases.Find(p => p.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(Phrases);
        }

        [HttpPost]
        public IActionResult Post(Phrase phrase)
        {
            if (string.IsNullOrEmpty(phrase.Author) || string.IsNullOrEmpty(phrase.Content))
            {
                return BadRequest();
            }

            Phrases.Add(phrase);

            var uri = Url.Action(nameof(GetById), "phrases", new { phrase.Id }, "https", Request.Host.ToUriComponent())
                ?? throw new InvalidOperationException();

            return Created(uri, new { phrase.Author, phrase.Content });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Phrase phrase)
        {
            if (string.IsNullOrEmpty(phrase.Author) || string.IsNullOrEmpty(phrase.Content))
            {
                return BadRequest();
            }

            phrase.Id = id;
            var findedPhrase = Phrases.Find(p => p.Id == id);

            if (findedPhrase != null)
            {
                findedPhrase.Author = phrase.Author;
                findedPhrase.Content = phrase.Content;

                var uri = Url.Action(nameof(GetById), "phrases", new { id }, "https", Request.Host.ToUriComponent())
                    ?? throw new InvalidOperationException();
                return Accepted(uri);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id, Phrase phrase)
        {
            if (phrase.Id == id)
            {
                var i = Phrases.FindIndex(p => p.Id == id);
                Phrases.RemoveAt(i);
                return Accepted();
            }
            return BadRequest();
        }
    }

    public static class StringExt
    {
        public static bool ContainsInsensitive(this string source, string search)
            => (new CultureInfo("pt-BR").CompareInfo)
                .IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
    }
}