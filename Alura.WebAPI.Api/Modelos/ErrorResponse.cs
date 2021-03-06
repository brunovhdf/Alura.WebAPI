using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.WebAPI.Api.Modelos
{
    public class ErrorResponse
    {
        public int Codigo { get; set; }
        public string Mensagem { get; set; }
        public ErrorResponse InnerError { get; set; }
        public string[] Detalhes { get; set; }
        public static ErrorResponse From(Exception e)
        {
            if (e == null)
                return null;
            return new ErrorResponse
            {
                Codigo = e.HResult,
                Mensagem = e.Message,
                InnerError = ErrorResponse.From(e.InnerException)
            };
        }

        public static ErrorResponse FromModelState(ModelStateDictionary modelState)
        {
            return new ErrorResponse
            {
                Codigo = 100,
                Mensagem = "ModelState is invalid",
                Detalhes = modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray()
            };
        }
    }
}
