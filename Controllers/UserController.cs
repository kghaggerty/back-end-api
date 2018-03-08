using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back_end_api.Data;

// GET api from User Model
namespace back_end_api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private back_end_apiContext _context;
        // Constructor method to create an instance of context to communicate with our database.
        public UserController(back_end_apiContext ctx)
        {
            _context = ctx;
        }
        // This method handles GET requests to GET a list of users 
        [HttpGet]
        public IActionResult Get()
        {
            var Users = _context.User.ToList();
            if (Users == null)
            {
                return NotFound();
            }
            return Ok(Users);
        }

        // This method is using GET to retrieve a single User
        [HttpGet("{id}", Name = "GetSingleUser")]
        public IActionResult Get(int id)
        {
            // error to handle if the user input the correct info in order to use the api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // search database to try and find a match for the computer id entered
            try
            {
                User User = _context.User.Single(g => g.UserId == id);

                if (User == null)
                {
                    return NotFound();
                }

                return Ok(User);
            }
            catch (System.InvalidOperationException ex)
            {
                Console.Write(ex);
                return NotFound();
            }
        }

        /* This method handles POST requests to add a user,
        saves it and throws an error if it already exists. */
        [HttpPost]
        public IActionResult Post([FromBody]User User)
        {
            // error to handle if the user input the correct info in order to use the api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // save user to BANGAZON_DB 
            _context.User.Add(User);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // check if the User Id already exists in the database and throw an error
                if (UserExists(User.UserId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("GetSingleUser", new { id = User.UserId }, User);
        }

        /* This method handles PUT requests to edit a single user through searching by id in the db,
        saves modifications and returns an error if the user does not exist. */
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]User User)
        {
            // error to handle if the user input the correct info in order to use the api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != User.UserId)
            {
                return BadRequest();
            }
            _context.User.Update(User);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        /* This method handles DELETE requests to delete a single user through searching by id in the db,
        removes user and returns an error if the user does not exist. */
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User User = _context.User.Single(g => g.UserId == id);

            if (User == null)
            {
                return NotFound();
            }
            _context.User.Remove(User);
            _context.SaveChanges();
            return Ok(User);
        }

        private bool UserExists(int UserId)
        {
            return _context.User.Any(g => g.UserId == UserId);
        }
        
    }
}