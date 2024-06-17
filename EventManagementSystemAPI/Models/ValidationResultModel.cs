using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventManagementSystemAPI.Model
{
    public class ValidationResultModel <T>
    {
        public string status { get; }

        public List<ValidationError> Errors { get; }

        public T? Data { get; }

        public string Message { get; }
        public ValidationResultModel(ModelStateDictionary modelState)
        {
            status = "400";
            Message = "One or more erros have occurred";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            Data = default(T);
        }
    }
    public class ValidationError
    {
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }

    }
}