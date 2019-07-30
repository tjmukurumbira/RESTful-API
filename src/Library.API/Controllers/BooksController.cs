using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
   
   [Route("api/authors/{authorId}/books")]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryRepository repository;

        public BooksController(ILibraryRepository repository)
        {
            this.repository = repository;
        }
     
       public IActionResult GetBooksForAuthor(Guid authorId)
       {
           if (!this.repository.AuthorExists(authorId))
            return NotFound();
          var books = this.repository.GetBooksForAuthor(authorId);

          var dtos = Mapper.Map<IEnumerable<BookDto>>(books);

          return Ok(dtos);
       }

        [HttpGet("{id}")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
       {
           if (!this.repository.AuthorExists(authorId))
            return NotFound();
          var book = this.repository.GetBookForAuthor(authorId,id);
          if (book==null)
                return NotFound();

          var dto = Mapper.Map<IEnumerable<BookDto>>(book);

          return Ok(dto);
       }
       
    }
}