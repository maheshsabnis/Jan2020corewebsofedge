using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core_WebApp.Models;
using Core_WebApp.Services;
namespace Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombineController : ControllerBase
    {
        private readonly IRepository<Category, int> catR;
        private readonly IRepository<Product, int> prdR;
        public CombineController(IRepository<Category, int> catR, IRepository<Product, int> prdR)
        {
            this.catR = catR;
            this.prdR = prdR;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ViewModel model)
        {
            model.Category = await catR.CreateAsync(model.Category);
            foreach (Product product in model.Products)
            {
                product.CategoryRowId = model.Category.CategoryRowId;
                await prdR.CreateAsync(product);
            }

            return Ok();
        }
    }
}