using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using PRSBackEndPT.models;

namespace PRSBackEndPT.Controllers
{
    [Route("/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PrsDbContext _context;
        private const String NEW = "New";
        private const String REVIEW = "Review";
        private const String APPROVED = "Approved";
        private const String REJECTED = "Rejected";
        private const String REOPENED = "Reopened";

        public RequestsController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: /requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.Include(r => r.User).ToListAsync();
        }

        // GET: /requests/(id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            //var request = await _context.Requests.FindAsync(id);

            var request = await _context.Requests.Where(r => r.Id == id)
                .Include(r => r.User)
                .FirstOrDefaultAsync();


            if (request == null)
            {
                return NotFound();
            }
            return request;
        }

        // POST: /requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // PUT: /requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutRequest(Request request)
        {
            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       

        // DELETE: /requests/(id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET LIST OF REQUESTS FOR REVIEW 
        [HttpGet("/list-review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetAllForReview(int userId)

        {
            return await _context.Requests.Include(r => r.User)
                .Where(r => r.Status.Equals("Review") && !r.UserId.Equals(userId)) 
                .ToListAsync();

        }

        // APPROVE
        [HttpPut("/approve")] 
        public async Task<ActionResult<Request>> Approve(Request approvedRequest)
        {
            var request = await _context.Requests.FindAsync(approvedRequest.Id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = APPROVED;

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return request;
        }

        // REJECT
        [HttpPut("/reject")]
        public async Task<ActionResult<Request>> Reject(Request rejectedRequest)
        {
            var request = await _context.Requests.FindAsync(rejectedRequest.Id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = REJECTED;

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return request;
        }

        // REOPEN
        [HttpPut("/reopen")]
        public async Task<ActionResult<Request>> Reopen(Request reopenedRequest)
        {
            var request = await _context.Requests.FindAsync(reopenedRequest.Id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = REOPENED;

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return request;
        }

        // SUBMIT
        [HttpPut("/submit")]
        public async Task<ActionResult<Request>> Submit(Request submittedRequest)
        {
            var request = await _context.Requests.FindAsync(submittedRequest.Id);

            if (request == null)
            {
                return NotFound();
            }

            request.SubmittedDate = DateTime.Now;

            if (request.Total > 50.00m) {
                request.Status = REVIEW;
            }
            else
            {
                request.Status = APPROVED;
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return request;
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }

        private void Recalc()
        {
            // not API
            //methods called from APIs that make changes
            // add a recalc method (requestID)
            // total all the lines
            // update Request.Total

            // sum up the request lines rl.Quantity rl.Product
            // update the request
            // save changes 
        }
    }
}
