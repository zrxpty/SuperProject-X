using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Tools.GenericModels;

namespace Tools.Controller
{
    public class DefaultController : ControllerBase
    {
        protected readonly ILogger<DefaultController> logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            this.logger = logger;
        }
        protected async Task<ActionResult> GetServiceResponseAsync<T>(Func<Task<ServiceResponse<T>>> serviceFunction) where T : class
        {
            try
            {
                var result = await serviceFunction();
                if (result.Code != 200)
                {
                    logger.LogError($"Code: {result.Code}, class: {serviceFunction.Method.DeclaringType}, method: {serviceFunction.Method.Name}, message: {result.Code}");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Code: 500, class: {serviceFunction.Method.DeclaringType}, method: {serviceFunction.Method.Name}, exception: {ex.Message}, stackTrace: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        protected async Task<ActionResult> GetServiceResponseWithAuthAsync<T>(Func<List<ClaimModel>, Task<ServiceResponse<T>>> serviceFunction) where T : class
        {
            logger.LogInformation($"Incoming request; service function: {GetMethod(serviceFunction)};");
            try
            {
                var user = await GetClaims();
                if (user != null)
                {
                    var result = await serviceFunction(user);
                    if (result.Code != 200)
                    {
                        logger.LogError($"Code: {result.Code}, class: {serviceFunction.Method.DeclaringType}, method: {serviceFunction.Method.Name}, message: {result.Message}, user: {user.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}");
                    }
                    logger.LogInformation($"Function {serviceFunction.Method.Name} executed");
                    return Ok(result);
                }
                logger.LogError($"Не удалось найти пользователя {User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value}");
                return StatusCode(401, "Unauthorized");
            }
            catch (Exception ex)
            {
                logger.LogError($"Code: 500, class: {serviceFunction.Method.DeclaringType}, method: {serviceFunction.Method.Name}, exception: {ex.Message}, stackTrace: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<List<ClaimModel>> GetClaims()
        {
            return await Task.FromResult(User.Claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value }).ToList());
        }

        private string GetMethod<T>(Func<List<ClaimModel>, Task<ServiceResponse<T>>> function) where T : class
        {
            return function.Method.Name;
        }


    }
    public class ClaimModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}