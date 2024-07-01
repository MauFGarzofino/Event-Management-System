namespace EventManagementSystemAPI.Models
{
    public class InsufficientTicketsException : Exception
    {
        public InsufficientTicketsException()
        {
        }

        public InsufficientTicketsException(string message)
            : base(message)
        {
        }

        public InsufficientTicketsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
