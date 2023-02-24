using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using PRSBackEndPT.models;

namespace PRSBackEndPT.Controllers
{
    [Route("api/[controller]")]
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

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
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
    }
}
