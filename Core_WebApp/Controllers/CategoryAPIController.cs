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
    [ApiController] // <--- Performs the Http request Body mapping for POST & PUT method 
    public class CategoryAPIController : ControllerBase
    {
        private readonly IRepository<Category, int> _catRepository;
        public CategoryAPIController(IRepository<Category, int> catRepository)
        {
            _catRepository = catRepository;
        }

        [HttpGet] // mapping of request to the current method
        public async Task<IActionResult> GetAsync()
        {
            var cats = await _catRepository.GetAsync();
            return Ok(cats); // JSON Serialized Response
        }
        // http://loaclhost:<PORT>/api/CategoryAPI/101
        [HttpGet("{id}")] // mapping of request to the current method with URL parameter
        public async Task<IActionResult> GetAsync(int id)
        {
            var cat = await _catRepository.GetAsync(id);
            return Ok(cat); // JSON Serialized Response
        }

        // Parameter Binders
        // FromBody --> Data will be mapped from Body
        // FromQuery --> Data will be mapped from Query String
        // FromForm --> Data will be accepted as FormPost and will be mapped
        // FormRoute --> Data will be mapped from Route values
       // [HttpPost]
        //    public async Task<IActionResult> PostAsync([FromBody]Category cat)
        [HttpPost]
        //public async Task<IActionResult> PostAsync(string CategoryId, string CategoryName, int BasePrice)
        //public async Task<IActionResult> PostAsync([FromQuery]Category cat)
        public async Task<IActionResult> PostAsync(Category cat)
        {
            //var cat = new Category()
            //{
            //     CategoryId = CategoryId,
            //     CategoryName = CategoryName,
            //     BasePrice = BasePrice
            //};
           
                if (ModelState.IsValid)
                {
                    if (cat.BasePrice < 0) throw new Exception("Base Price cannot be -ve");
                    cat = await _catRepository.CreateAsync(cat);
                    return Ok(cat);
                }
                return BadRequest(ModelState); // respond the validation error
            
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Category cat)
        {
            if (ModelState.IsValid)
            {
                cat = await _catRepository.UpdateAsync(id, cat);
                return Ok(cat);
            }
            return BadRequest(ModelState); // respond the validation error
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
                var resp = await _catRepository.DeleteAsync(id);
            if (resp == false) return NotFound("Record Nor Found");    
                return Ok(resp);
        }

    }
}