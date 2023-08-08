using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Core.CrossCuttingConcerns.Exceptions.HttpExceptions
{
    internal class ValidationProblemDetails : ProblemDetails
    {
        public object Failures { get; init; }
        public ValidationProblemDetails(string detail, object failures)
        {
            Title = "Validation Error";
            Detail = detail;
            Status = StatusCodes.Status403Forbidden;
            Type = "Validation Error. Invalid input type for the entity";
            Failures = failures;
            Instance = "";

        }
    }
}
