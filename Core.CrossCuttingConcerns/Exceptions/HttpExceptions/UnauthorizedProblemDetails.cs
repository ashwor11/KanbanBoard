using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcerns.Exceptions.HttpExceptions;

public class UnauthorizedProblemDetails : ProblemDetails
{
    public UnauthorizedProblemDetails(string detail)
    {
        Status = StatusCodes.Status403Forbidden;
        Title = "Authorization Error";
        Detail = detail;
        Type = "https://example.com/probs/authorization";
        Instance = "";
        
    }
}