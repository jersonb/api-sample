using System.Globalization;
using System.Text.Json;
using ExampleToday.Api.Models;
using ExampleToday.Api.ViewObjects;
using Microsoft.AspNetCore.Mvc;

namespace ExampleToday.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PhrasesController : ControllerBase
    {
        private static readonly List<Phrase> Phrases = DataMock.Data;

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<IEnumerable<Phrase>>))]
        public IActionResult GetAll()
        {
            return Ok(new SuccessResponse<IEnumerable<Phrase>>(Phrases));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<IEnumerable<Phrase>>))]
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
            return Ok(new SuccessResponse<IEnumerable<Phrase>>(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<Phrase>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(FailResponse))]
        public IActionResult GetById(int id)
        {
            var result = Phrases.Find(p => p.Id == id);
            if (result == null)
            {
                return NotFound(new FailResponse($"Not Found id {id}"));
            }
            return Ok(new SuccessResponse<Phrase>(result));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResponse<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailResponse))]
        public IActionResult Post(Phrase phrase)
        {
            if (string.IsNullOrEmpty(phrase.Author) || string.IsNullOrEmpty(phrase.Content))
            {
                return BadRequest(new FailResponse($"Invalid data {JsonSerializer.Serialize(phrase, new JsonSerializerOptions { WriteIndented = true })}"));
            }

            Phrases.Add(phrase);

            var uri = Url.Action(nameof(GetById), "phrases", new { phrase.Id }, "https", Request.Host.ToUriComponent())
                ?? throw new InvalidOperationException();

            return Created(uri, new { phrase.Author, phrase.Content });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SuccessResponse<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(FailResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailResponse))]
        public IActionResult Put(int id, Phrase phrase)
        {
            if (string.IsNullOrEmpty(phrase.Author) || string.IsNullOrEmpty(phrase.Content))
            {
                return BadRequest(new FailResponse($"Invalid data {JsonSerializer.Serialize(phrase, new JsonSerializerOptions { WriteIndented = true })}"));
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
            return NotFound(new FailResponse($"Not Found id {id}"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(SuccessResponse<object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(FailResponse))]
        public IActionResult Delete(int id, Phrase phrase)
        {
            if (phrase.Id == id)
            {
                var i = Phrases.FindIndex(p => p.Id == id);
                Phrases.RemoveAt(i);
                return Accepted();
            }
            return BadRequest(new FailResponse($"Invalid data {JsonSerializer.Serialize(phrase, new JsonSerializerOptions { WriteIndented = true })}"));
        }
    }

    public static class StringExt
    {
        public static bool ContainsInsensitive(this string source, string search)
            => (new CultureInfo("pt-BR").CompareInfo)
                .IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
    }
}