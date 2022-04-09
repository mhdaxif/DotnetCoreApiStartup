using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    //public class GreetFilter<T> : IActionFilter where T : class
    public class GreetFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new NotFoundObjectResult(null);
            context.Result = new NoContentResult();
            context.Result = new BadRequestResult();
            context.Result = new OkResult();

            context.Result = new OkObjectResult(
                new List<object> { }
                ); 
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // context.Result = new NotFoundResult();
        }

    }
}