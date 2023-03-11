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
    [Route("/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public ProductsController(PrsDbContext context)
        {
            _context = context;
        }

        // GET: /products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(r => r.Vendor).ToListAsync();
        }



        // GET: /products/(id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            //var product = await _context.Products.FindAsync(id);

            var product = await _context.Products.Where(r => r.Id == id)
                .Include(r => r.Vendor)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: /products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var createdProduct = await _context.Products.Where(r => r.Id == product.Id)
                .Include(r => r.Vendor)
                .FirstOrDefaultAsync();

            return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: /products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<Product>> PutProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updatedProduct = await _context.Products.Where(r => r.Id == product.Id)
                .Include(r => r.Vendor)
                .FirstOrDefaultAsync();

            return updatedProduct;
        }

        // DELETE: /products/(id)
        [HttpDelete("{id}")]
        public async Task<ActionResult<String>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return "Product deleted";
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
