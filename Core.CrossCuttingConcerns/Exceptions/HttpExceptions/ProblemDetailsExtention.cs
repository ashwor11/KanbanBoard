using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Exceptions.HttpExceptions
{
    internal static class ProblemDetailsExtention
    {
        public static string AsJson(this ProblemDetails details) { return JsonConvert.SerializeObject(details); }
    }
}
