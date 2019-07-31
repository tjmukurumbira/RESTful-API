using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
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

        [HttpGet("{id}", Name="GetBookForAuthor")]
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

       [HttpPost]
        public IActionResult CreateBookForAuthor( Guid authorId,[FromBody] CreateBookDto book)
        {
            if (book== null)
              return BadRequest();

             if (! ModelState.IsValid){
                 return 
             }

            if (!this.repository.AuthorExists(authorId))
              return NotFound();

            var bookEntity = Mapper.Map<Book>(book);
            this.repository.AddBookForAuthor(authorId,bookEntity);
            if (!this.repository.Save())
            {
             throw new Exception("A problem occured while handing your request");
            }

            var bookToReturn = Mapper.Map<BookDto>(bookEntity);
            return CreatedAtRoute("GetBookForAuthor", new {authorId= bookToReturn.AuthorId, id = bookToReturn.Id}, bookToReturn);

        } 

        [HttpDelete]
        public IActionResult DeleteBookForAuthor( Guid authorId,Guid bookId)
        {
            
            if (!this.repository.AuthorExists(authorId))
              return NotFound();

            var book = this.repository.GetBookForAuthor(authorId, bookId);
            if (book ==null)
             return NotFound();

             this.repository.DeleteBook(book);
            
            if (!this.repository.Save())
            {
             throw new Exception("A problem occured while handing your request");
            }
            return NoContent();
        } 

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id , [FromBody] UpdateBookDto book)
        {
            if (book== null)
              return BadRequest();

            if (!this.repository.AuthorExists(authorId))
              return NotFound();

            var bookEntity = this.repository.GetBookForAuthor(authorId, id);
            if (bookEntity== null)
                return NotFound();
            
            Mapper.Map(book, bookEntity);

            this.repository.UpdateBookForAuthor(bookEntity);

            if (!this.repository.Save())
            {
             throw new Exception($"Updating book {id}  for author {authorId} failed");
            }
            
          return NoContent();
        }
       
    }
}