using ECommerce.Shared.CommonResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ApiBaseController:ControllerBase
    {

        //Result : [Without Data]
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else 
                return HandleProblem(result.Errors);
        }
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandleProblem(result.Errors);
        }

        #region Helper Methods
        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            //1- No Error [] => Server Error 500
            if (errors.Count == 0)
                return Problem(title: "An Error Occured", statusCode: StatusCodes.Status500InternalServerError);

            //2-Errors => Validation Errors => Validation  Problem
            if (errors.Any(e => e.ErrorType == ErrorType.Validation))
                return HandleValidationErrors(errors);

            //3-Single Error => Not Found , UnAuthorized , ....... [Switch]
            return HandleSingleError(errors[0]);

        }

        private ActionResult HandleSingleError(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Descreption,
                type: error.ErrorType.ToString(),
                statusCode: MapErrorTypeIntoStatusCode(error.ErrorType)
                );
        }
        private static int MapErrorTypeIntoStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.ForBidden => StatusCodes.Status403Forbidden,
                ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };
        }

        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)
        {
            var modelstate = new ModelStateDictionary();
            foreach (var error in errors)
                modelstate.AddModelError(error.Code, error.Descreption);
            return ValidationProblem(modelstate);
        } 
        #endregion
    }
}
