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
    [Route("/api/Goals")]
    public class GoalsController : Controller
    {
        private back_end_apiContext _context;
        private readonly UserManager<User> _userManager;
        // Constructor method to create an instance of context to communicate with our database.
        public GoalsController(back_end_apiContext ctx, UserManager<User> userManager)
        {
            _context = ctx;
            _userManager = userManager;
        }
        // This method handles GET requests to GET a list of users 
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            User user = await _context.User.Where(u => u.UserName == User.Identity.Name).SingleOrDefaultAsync();
            var Goals = _context.Goals.Include("User").Where(u => u.User.Id == user.Id ).ToList();
            if (Goals == null)
            {
                return NotFound();
            }
            return Ok(Goals);
        }

        // This method is using GET to retrieve a single User
        [HttpGet("{id}", Name = "GetSingleGoals")]
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
                Goals Goals = _context.Goals.Single(g => g.GoalsId == id);

                if (Goals == null)
                {
                    return NotFound();
                }

                return Ok(Goals);
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
        public async Task<IActionResult> Post([FromBody]Goals Goals)
        {
            ModelState.Remove("User");
            // error to handle if the user input the correct info in order to use the api
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Grab current user
            User user = await _context.User.Where(u => u.UserName == User.Identity.Name).SingleOrDefaultAsync();
            
            Goals.User = user;

            //Save to database
            _context.Goals.Add(Goals);
    
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // check if the User Id already exists in the database and throw an error
                if (UserExists(Goals.GoalsId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("GetSingleGoals", new { id = Goals.GoalsId }, Goals);
        }

        /* This method handles PUT requests to edit a single user through searching by id in the db,
        saves modifications and returns an error if the user does not exist. */
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id)
        
        {
            var goals = await _context.Goals.SingleOrDefaultAsync(m => m.GoalsId == id);
            goals.isCompleted = true;
            
            try
            {
                    _context.Update(goals);
                    await _context.SaveChangesAsync();
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
            Goals Goals = _context.Goals.Single(g => g.GoalsId == id);

            if (User == null)
            {
                return NotFound();
            }
            _context.Goals.Remove(Goals);
            _context.SaveChanges();
            return Ok(User);
        }

        private bool UserExists(int GoalsId)
        {
            return _context.Goals.Any(g => g.GoalsId == GoalsId);
        }
        
    }
}