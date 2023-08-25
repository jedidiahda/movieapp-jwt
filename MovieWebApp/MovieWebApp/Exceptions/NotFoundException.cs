using System.Net;

namespace MovieWebApp.Exceptions
{
    public class NotFoundException: CustomException
    {
        public NotFoundException(string message)
            : base(message, null, HttpStatusCode.NotFound)
        {
        }
    }
}
