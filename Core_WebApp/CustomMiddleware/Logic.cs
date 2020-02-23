using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.CustomMiddleware
{
	/// <summary>
	/// The Class for Defining Schema for Error Response
	/// </summary>
	public class ErrorInformation
	{
		public int ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
	}

	/// <summary>
	/// The Custom Middleware Logic class
	/// 1. Constructor Injected with the RequestDelegate
	/// 2. Have public InvokeAsync() method that contains middleware Logic
	/// </summary>
	public class ErrorMiddlware
	{
		private readonly RequestDelegate _request;

		public ErrorMiddlware(RequestDelegate request)
		{
			_request = request;
		}

		/// <summary>
		/// The Logic Method
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				// if eveything is fine move to next middleware
				await _request(context);
			}
			catch (Exception ex)
			{
				// logic to catch execption and generate response
				await HandleError(context, ex);
			}
		}

		private async Task HandleError(HttpContext ctx, Exception ex)
		{
			// set the error code
			ctx.Response.StatusCode = 500;

			// the ErrorInfromation class to define Error Response Schema
			var errorInfo = new ErrorInformation()
			{ 
				 ErrorCode = ctx.Response.StatusCode,
				 ErrorMessage = ex.Message
			};

			// write the response
			string errorMessage = JsonConvert.SerializeObject(errorInfo);
			await ctx.Response.WriteAsync(errorMessage);
		}
	}

	/// <summary>
	///  Class that will register the ErrorMiddleware class in Http Pipeline
	/// </summary>
	public static class ErrorMiddlwareExtension
	{
		public static void UseCustomErrorMiddleware(this IApplicationBuilder app)
		{
			// app.UseMiddleware<T>()
			// T is a Type that is ctor injected with RequestDelegate
			// and having the Invoke() or InvokeAsync() method
			app.UseMiddleware<ErrorMiddlware>();
		}
	}


}
