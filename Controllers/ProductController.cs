using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryAPI.Data;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : Controller
    {
        private readonly InventoryDbContext _context;

        public ProductController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("TestActionMethod")]
        public IActionResult TestAction()
        {
            return Ok("Everything is fine.....!");
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product productItem)
        {
            if (ModelState.IsValid)
            {
                await _context.Products.AddAsync(productItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSingleProduct", new { productItem.Id }, productItem);
            }

            return new JsonResult("Error, Something is wrong")
            {
                StatusCode = 500
            };
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSingleProduct(int id)
        {
            var productItem = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productItem == null)
                return NotFound();

            return Ok(productItem);
        }

        [HttpPut("EditProduct/{Id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] Product productItem)
        {
            if (id != productItem.Id)
                return BadRequest();
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null)
                return NotFound();

            existingProduct.ProductName = productItem.ProductName;
            existingProduct.Desc = productItem.Desc;
            existingProduct.Price = productItem.Price;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
