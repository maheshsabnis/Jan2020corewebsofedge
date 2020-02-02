using Core_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.CustomFilters
{
	public class MyExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private readonly IModelMetadataProvider modelMetadataProvider;
		 
		
		/// <summary>
		/// IModelMetadataProvider, will be resolved by MvcOptions class
		/// </summary>
		/// <param name="modelMetadataProvider"></param>
		/// 
		public MyExceptionFilterAttribute(IModelMetadataProvider modelMetadataProvider)
		{
			this.modelMetadataProvider = modelMetadataProvider;
		}
		public override void OnException(ExceptionContext context)
		{
			// 1. Handle the Exception
			context.ExceptionHandled = true;
			// 2. Read Exception Meesage
			string message = context.Exception.Message;
			// 3 Set the Result
			// 3a. Set the ViewReult to show Error Page
			var viewResult = new ViewResult();
			// 3b. set ViewData that will carry data to be shown on error page 
			var viewData = new ViewDataDictionary(modelMetadataProvider, context.ModelState);
			viewData["controller"] = context.RouteData.Values["controller"];
			viewData["action"] = context.RouteData.Values["action"];
			viewData["errorMessage"] = message;
			viewResult.ViewData = viewData;

			viewResult.ViewName = "CustomError";
			context.Result = viewResult;
		}
	}
}
