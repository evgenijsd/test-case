using System.Net;
using test_case.api.Enums;
using test_case.api.Exceptions;

namespace test_case.api.Extensions
{
    public static class ExceptionFilterExtensions
    {
        public static (HttpStatusCode statusCode, ErrorCode errorCode) ParseException(this Exception exception)
        {
            return exception switch
            {
                NotFoundException _ => (HttpStatusCode.NotFound, ErrorCode.NotFound),
                LoginExistsException _ => (HttpStatusCode.Unauthorized, ErrorCode.LoginExists),
                InvalidTokenException _ => (HttpStatusCode.Unauthorized, ErrorCode.InvalidToken),
                NoFileException _ => (HttpStatusCode.BadRequest, ErrorCode.NoFile),
                _ => (HttpStatusCode.InternalServerError, ErrorCode.General),
            };
        }
    }
}
