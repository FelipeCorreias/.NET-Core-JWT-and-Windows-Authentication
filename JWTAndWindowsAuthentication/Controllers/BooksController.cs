using JWTAndWindowsAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JWTAndWindowsAuthentication.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(AuthenticationSchemes = "Windows")] //Remove to disable windows auth and use only bearer auth
    public class BooksController : Controller
    {
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            var resultBookList = new Book[] {
                new Book { Author = "Miguel de Cervantes", Title = "Don Quixote"},
                new Book { Author = "Charles Dickens", Title = "Um Conto de Duas Cidades" },
                new Book { Author = "Paulo Coelho", Title = "O alquimista" },
                new Book { Author = "J. R. R. Tolkien", Title = "O Senhor dos Anéis" },
                new Book { Author = "Antoine de Saint-Exupéry", Title = "O Pequeno Príncipe" }
            };

            return resultBookList;
        }

        
    }
}
