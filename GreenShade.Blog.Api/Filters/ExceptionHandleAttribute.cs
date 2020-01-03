using GreenShade.Blog.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShade.Blog.Api.Filters
{
    public class ExceptionHandleAttribute: ExceptionFilterAttribute
    {
        private readonly string _message;
        public ExceptionHandleAttribute(string message)
        {
            _message = message;
        }
        public ExceptionHandleAttribute()
        {

        }
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled != true)
            {
                context.Result = new JsonResult(new ApiResult() { Code = System.Net.HttpStatusCode.NoContent, Msg = _message, Result = "" });
                context.ExceptionHandled = true;
            }             
        }
    }
}
