using GATEWAY.Models;
using Microsoft.AspNetCore.Mvc;

namespace GATEWAY.Controllers.CustomController
{
    public class DefaultController : ControllerBase
    {
        protected async Task<ActionResult> GetResponse<T>(Func<Task<Response<T>>> serviceFunction)
        {
            try
            {
                var result = await serviceFunction();
                if (result.Code != 200)
                {
                    return StatusCode(result.Code, result.Message);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        protected async Task<ActionResult> GetResponseWithAuth<T>(Func<Task<Response<T>>> serviceFunction)
        {
            try
            {
                var result = await serviceFunction();
                if (result.Code != 200)
                {
                    return StatusCode(result.Code, result.Message);
                }
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
