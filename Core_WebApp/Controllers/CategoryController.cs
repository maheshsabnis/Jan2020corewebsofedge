using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core_WebApp.Models;
using Core_WebApp.Services;

namespace Core_WebApp.Controllers
{
    /// <summary>
    /// Controller is a base class for ASP.NET Core MVC Controller for follwoing
    /// IActionFilter , the interface that will be used to define functionality
    /// to be added in Http Request wheb controller starts executing e.g. Exceptions
    /// IFilterMetadata, used by Filter to work with Model Metadata for Data Validation
    /// IAsyncActionFilter, the Async Filters, IDsiposable
    /// Controller have all methods as HttpGet by default to accept the request and execute
    /// To accept POSt request method must be decoarated with HttpPost attribute
    /// Each HttpGet and HttpPost request is called 'ActionMethod' and its return 
    /// IActionresult (ViewResult/PartialViewResult/JsonResult/FileResult)
    /// </summary>
    public class CategoryController : Controller
    {
        // inject the Category Repository
        private IRepository<Category, int> repository;
        public CategoryController(IRepository<Category, int> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Return the List of Model Data, in our case it will be Category
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var res = await repository.GetAsync();
            return View(res); // this will return Index View as the name of method is 'Index'
        }

        /// <summary>
        /// Return an empty Category Object to create new Category 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var res = new Category();
            return View(res);
        }
        /// <summary>
        /// This will accept the Category Object to create a new Category 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async  Task<IActionResult> Create(Category category)
        {
            //try
            //{
                // validate the received category
                if (ModelState.IsValid)
                {
                    if (category.BasePrice < 0)
                        throw new Exception("Base Price Cannot be -ve");
                    var res = await repository.CreateAsync(category);
                    return RedirectToAction("Index"); //redirect to Index action method
                }
                return View(category); // stay on Create View with errors
            //}
            //catch (Exception ex)
            //{
            //    return View("Error", new ErrorViewModel()
            //    {
            //        ControllerName = RouteData.Values["controller"].ToString(),
            //        ActionName = RouteData.Values["action"].ToString(),
            //        ErrorMessage = ex.Message
            //    }); 
            //}
        }

        /// <summary>
        /// Return an empty Category Object to create new Category 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            var res = await repository.GetAsync(id);
            return View(res); // return Edit view with data to be edited
        }
        /// <summary>
        /// This will accept the Category Object to create a new Category 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            // validate the received category
            if (ModelState.IsValid)   
            {
                var res = await repository.UpdateAsync(id,category);
                return RedirectToAction("Index"); //redirect to Index action method
            }
            return View(category); // stay on edit View with errors
        }

        public async Task<IActionResult> Delete(int id)
        {
            var res = await repository.DeleteAsync(id);
            if (res)
            {
                return RedirectToAction("Index"); // with success in delete
            }
            return RedirectToAction("Index"); // with fail in delete
        }

        public IActionResult ShowProducts(int id)
        {
            TempData["CatRowId"] = id;
            return RedirectToAction("Index", "Product"); 
        }


    }
}