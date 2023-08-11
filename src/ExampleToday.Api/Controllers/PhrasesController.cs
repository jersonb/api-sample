using System.Globalization;
using ExampleToday.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleToday.Api.Controllers
{
    [ApiController]
    [Route("phrases")]
    public class PhrasesController : ControllerBase
    {
        private static readonly List<Phrase> Phrases = DataMock.Data;

        [HttpGet]
        public IEnumerable<Phrase> Get(string? author, string? term, int page = 1, int limit = 10)
        {
            var phrases = Phrases.AsEnumerable();

            if (author != null)
            {
                phrases = phrases.Where(p => p.Author.ContainsInsensitive(author));
            }

            if (term != null)
            {
                phrases = phrases.Where(p => p.Content.ContainsInsensitive(term));
            }

            page = (page * limit) - limit;

            return phrases.Skip(page).Take(limit);
        }

        [HttpGet("{id}")]
        public Phrase? GetById(int id)
        {
            return Phrases.Find(p => p.Id == id);
        }

        [HttpGet("all")]
        public List<Phrase> GetAll()
        {
            return Phrases;
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

    public static class StringExt
    {
        public static bool ContainsInsensitive(this string source, string search)
            => (new CultureInfo("pt-BR").CompareInfo)
                .IndexOf(source, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;
    }
}