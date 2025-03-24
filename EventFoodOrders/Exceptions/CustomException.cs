using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventFoodOrders.Exceptions;

public class CustomException : Exception
{
    public int Code;

    public CustomException(int? statusCode = null, string? message = "") : base(message)
    {
        Code = statusCode ?? StatusCodes.Status500InternalServerError;
    }

    public CustomException(string message, Exception exception) : base(message, exception)
    { }
}
