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
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryRepository repository;

        public AuthorsController(ILibraryRepository repository)
        {
            this.repository = repository;
        }
     
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = this.repository.GetAuthors();
            var dtos = Mapper.Map<IEnumerable<AuthorDto>>(authors);            
            return  Ok(dtos);
        }

        [HttpGet("{id}",Name="GetAuthor")]
        public IActionResult GetAuthor(Guid id)
        {
            var author = this.repository.GetAuthor(id);
            if (author==null)
             {
                 return NotFound();
             } 
            var dto = Mapper.Map<AuthorDto>(author);            
            return Ok(dto);
        }
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] CreateAuthorDto author)
        {
            if (author== null)
              return BadRequest();

            var authorEntity = Mapper.Map<Author>(author);
            this.repository.AddAuthor(authorEntity);
            if (!this.repository.Save())
            {
             throw new Exception("A problem occured while handing your request");
            }

            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", new {id = authorToReturn.Id}, authorToReturn);

        } 
        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            var author = this.repository.GetAuthor(id);
            if (author==null)
             return NotFound();
            this.repository.DeleteAuthor(author);
             if (!this.repository.Save())
            {
             throw new Exception("A problem occured while handing your request");
            }

            return NoContent();

        }
    }
}