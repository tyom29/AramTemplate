using ErrorOr;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Aram.BFF.Api.Base;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        return errors.All(error => error.Type == ErrorType.Validation) ? ValidationProblem(errors) : Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        int statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, detail: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

        foreach (Error error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}