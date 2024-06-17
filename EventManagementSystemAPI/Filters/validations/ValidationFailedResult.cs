using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using EventManagementSystemAPI.Models;
using EventManagementSystemAPI.Model;

namespace EventManagementSystemAPI.Filters.validations
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationResultModel<string>(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
