using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSBackEndPT.models;

namespace PRSBackEndPT.Controllers
{
    [Route("/request-lines")]
    [ApiController]
    public class RequestLinesController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestLinesController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: /request-lines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLine()
        {
            return await _context.RequestLine
                .Include(r => r.Request).ThenInclude(request => request.User)
                .Include(r => r.Product).ThenInclude(product => product.Vendor)
                .ToListAsync();
        }

        // GET: /request-lines/(id)
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLine
               .Where(r => r.Id == id)
               .Include(r => r.Request).ThenInclude(request => request.User)
               .Include(r => r.Product).ThenInclude(product => product.Vendor)
               .FirstOrDefaultAsync();

            if (requestLine == null)
            {
                return NotFound();
            }

            return requestLine;
        }

        // POST: /request-lines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLine.Add(requestLine);

            await _context.SaveChangesAsync();

            await RecalculateRequestTotal(requestLine.RequestId);

            var createdRequestLine = await _context.RequestLine
               .Where(r => r.Id == requestLine.Id)
               .Include(r => r.Request).ThenInclude(request => request.User)
               .Include(r => r.Product).ThenInclude(product => product.Vendor)
               .FirstOrDefaultAsync();

            return CreatedAtAction("GetRequestLine", new { id = createdRequestLine.Id }, createdRequestLine);
        }

        // PUT: /request-lines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<RequestLine>> PutRequestLine(RequestLine requestLine)
        {
            _context.Entry(requestLine).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(requestLine.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await RecalculateRequestTotal(requestLine.RequestId);

            var updatedRequestLine = await _context.RequestLine
                .Where(r => r.Id == requestLine.Id)
                .Include(r => r.Request).ThenInclude(request => request.User)
                .Include(r => r.Product).ThenInclude(product => product.Vendor)
                .FirstOrDefaultAsync();

            return updatedRequestLine;
        }



        // DELETE: /request-lines/(id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLine.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }

            _context.RequestLine.Remove(requestLine);
            await _context.SaveChangesAsync();

            await RecalculateRequestTotal(requestLine.RequestId);

            return NoContent();
        }

        // GET LIST OF REQUESTLINES BY REQUESTID
        [HttpGet("/lines-for-request/{id}")]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetByRequestId(int id)
        {
            return await _context.RequestLine
                .Include(r => r.Product).ThenInclude(product => product.Vendor)
                .Include(r => r.Request).ThenInclude(request => request.User)
                .Where(r => r.RequestId.Equals(id))
                .ToListAsync();
        }

        private async Task<IActionResult> RecalculateRequestTotal(int requestId) //add a recalc method (requestID)
        {
            var total = await _context.RequestLine
                .Where(rl => rl.RequestId == requestId)
                .Include(rl => rl.Product)
                .Select(rl => new { linetotal = rl.Quantity * rl.Product.Price }) // sum up the request lines rl.Quantity rl.Product
                .SumAsync(s => s.linetotal);

            var request = await _context.Requests.FindAsync(requestId);
            request.User = await _context.Users.FindAsync(request.UserId);

            request.Total = total; // update Request.Total
            _context.Entry(request).State = EntityState.Modified;

            await _context.SaveChangesAsync();   // save changes 

            return NoContent();
        }
        
        private bool RequestLineExists(int id)
        {
            return _context.RequestLine.Any(e => e.Id == id);
        }
    }
}
