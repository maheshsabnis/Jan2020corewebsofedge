using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.CustomFilters
{
	public class LogActionFilter : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
			 
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			 
		}
	}
}
