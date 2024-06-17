using EventManagementSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventManagementSystemAPI.Filters
{
    public class ResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is ValidationProblemDetails validationProblemDetails)
            {
                var apiResponse = Response<Dictionary<string, string[]>>.CreateError(
                    objectResult.StatusCode ?? 400,
                    "Validation failed. Please check the provided data.",
                    validationProblemDetails.Errors
                );

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
