using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions.HttpExceptions
{
    internal class BusinessProblemDetails : ProblemDetails
    {
        public BusinessProblemDetails(string detail)
        {
            Title = "Business Rule violation";
            Detail = detail;
            Status = StatusCodes.Status400BadRequest;
            Type = "Business Rule violation";
            Instance = "";
        }
    }
}
