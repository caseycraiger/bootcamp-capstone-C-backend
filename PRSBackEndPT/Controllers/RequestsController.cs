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
            var createdRequest = await _context.Requests.Where(r => r.Id == request.Id)
                .Include(r => r.User)
                .FirstOrDefaultAsync();
                createdRequest.Status = NEW;
            return CreatedAtAction("GetRequest", new { id = createdRequest.Id }, createdRequest);
        }

        // PUT: /requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<Request>> PutRequest(Request request)
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
            var updatedRequest = await _context.Requests.Where(r => r.Id == request.Id)
              .Include(r => r.User)
              .FirstOrDefaultAsync();

            return updatedRequest;
        }

       

        // DELETE: /requests/(id)
        [HttpDelete("{id}")]
        public async Task<ActionResult<String>> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return "Request deleted";
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
        public async Task<ActionResult<Request>> Approve(Request request)
        {
            //var approvedRequest = await _context.Requests.FindAsync(approvedRequest.Id);

            var approvedRequest = await _context.Requests.Where(r => r.Id == request.Id)
              .Include(r => r.User)
              .FirstOrDefaultAsync();

            if (approvedRequest == null)
            {
                return NotFound();
            }

            approvedRequest.Status = APPROVED;

            //_context.Entry((Request)approvedRequest).State = EntityState.Modified;
            _context.Entry(approvedRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(approvedRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return approvedRequest;
        }

        // REJECT
        [HttpPut("/reject")]
        public async Task<ActionResult<Request>> Reject(Request request)
        {

            var rejectedRequest = await _context.Requests.Where(r => r.Id == request.Id)
              .Include(r => r.User)
              .FirstOrDefaultAsync();

            if (rejectedRequest == null)
            {
                return NotFound();
            }

            rejectedRequest.Status = REJECTED;

            _context.Entry(rejectedRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(rejectedRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return rejectedRequest;
        }

        // REOPEN
        [HttpPut("/reopen")]
        public async Task<ActionResult<Request>> Reopen(Request request)
        {
            var reopenedRequest = await _context.Requests.Where(r => r.Id == request.Id)
              .Include(r => r.User)
              .FirstOrDefaultAsync();

            if (reopenedRequest == null)
            {
                return NotFound();
            }

            reopenedRequest.Status = REOPENED;

            _context.Entry(reopenedRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(reopenedRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return reopenedRequest;
        }

        // SUBMIT
        [HttpPut("/submit")]
        public async Task<ActionResult<Request>> Submit(Request request)
        {
            var submittedRequest = await _context.Requests.Where(r => r.Id == request.Id)
              .Include(r => r.User)
              .FirstOrDefaultAsync();

            if (submittedRequest == null)
            {
                return NotFound();
            }

            submittedRequest.SubmittedDate = DateTime.Now;

            if (submittedRequest.Total > 50.00m) {
                submittedRequest.Status = REVIEW;
            }
            else
            {
                submittedRequest.Status = APPROVED;
            }

            _context.Entry(submittedRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(submittedRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return submittedRequest;
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
