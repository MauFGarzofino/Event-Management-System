
namespace EventManagementSystemAPI.Models
{
    public class Response <T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }

        private Response(int status, string message, T data, IDictionary<string, string[]> errors)
        {
            Status = status;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public static Response<T> CreateSuccess(int status, string message, T data)
        {
            return new Response<T>(status, message, data, null);
        }

        public static Response<T> CreateError(int status, string message, IDictionary<string, string[]> errors)
        {
            return new Response<T>(status, message, default(T), errors);
        }

    }
}
