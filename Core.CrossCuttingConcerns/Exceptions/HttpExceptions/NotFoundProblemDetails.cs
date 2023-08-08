using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions.HttpExceptions
{
    internal class NotFoundProblemDetails : ProblemDetails
    {
        public NotFoundProblemDetails(string message)
        {
            Title = "Not Found";
            Detail = message;
            Type = "There is no record that requested.";
            Detail = "";
            Instance = "";
            Status = StatusCodes.Status404NotFound;

        }
    }
}
