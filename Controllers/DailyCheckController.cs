using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using back_end_api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

// GET api from Daily Check Model
namespace back_end_api.Controllers
{
    [Route("/api/DailyCheck")]
    public class DailyCheckController : Controller
    {
        private back_end_apiContext _context;
        private readonly UserManager<User> _userManager;
        // Constructor method to create an instance of context to communicate with our database.
        public DailyCheckController(back_end_apiContext ctx, UserManager<User> userManager)
        {
            _context = ctx;
            _userManager = userManager;
        }
        // This method handles GET requests to GET a list of users 
        [HttpGet]
        public IActionResult Get()
        {
            var DailyCheck = _context.DailyCheck.ToList();
            if (DailyCheck == null)
            {
                return NotFound();
            }
            return Ok(DailyCheck);
        }

        // This method is using GET to retrieve a single User
        [HttpGet("{id}", Name = "GetSingleDailyCheck")]
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
                DailyCheck DailyCheck = _context.DailyCheck.Single(g => g.DailyCheckId == id);

                if (DailyCheck == null)
                {
                    return NotFound();
                }

                return Ok(DailyCheck);
            }
            catch (System.InvalidOperationException ex)
            {
                Console.Write(ex);
                return NotFound();
            }
        }

        /* This method handles POST requests to add a user,
        saves it and throws an error if it already exists. */
        [Authorize]
        [HttpPost]
        [System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IActionResult> Post([FromBody]DailyCheck DailyCheck)
        {
            ModelState.Remove("User");
            // error to handle if the user input the correct info in order to use the api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Grab current user
            User user = await _context.User.Where(u => u.UserName == User.Identity.Name).SingleOrDefaultAsync();
            
            DailyCheck.User = user;

            //Save to database
            _context.DailyCheck.Add(DailyCheck);
    
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // check if the User Id already exists in the database and throw an error
                if (UserExists(DailyCheck.DailyCheckId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("GetSingleDailyCheck", new { id = DailyCheck.DailyCheckId }, DailyCheck);
        }

        /* This method handles PUT requests to edit a single user through searching by id in the db,
        saves modifications and returns an error if the user does not exist. */
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DailyCheck DailyCheck)
        {
            // error to handle if the user input the correct info in order to use the api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != DailyCheck.DailyCheckId)
            {
                return BadRequest();
            }
            _context.DailyCheck.Update(DailyCheck);
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
            DailyCheck DailyCheck = _context.DailyCheck.Single(g => g.DailyCheckId == id);

            if (User == null)
            {
                return NotFound();
            }
            _context.DailyCheck.Remove(DailyCheck);
            _context.SaveChanges();
            return Ok(User);
        }

        private bool UserExists(int DailyCheckId)
        {
            return _context.DailyCheck.Any(g => g.DailyCheckId == DailyCheckId);
        }
        
    }
}