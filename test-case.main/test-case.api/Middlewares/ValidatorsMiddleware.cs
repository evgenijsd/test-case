using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using test_case.api.Models.DTO;
using Newtonsoft.Json;

namespace test_case.api.Middlewares
{
    public class ValidatorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Dictionary<Type, object> _validators;

        public ValidatorMiddleware(RequestDelegate next, Dictionary<Type, object> validators)
        {
            _next = next;
            _validators = validators;
        }

        public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
        {
            var actionArguments = context.GetEndpoint()?.Metadata?.GetMetadata<ControllerActionDescriptor>()?.MethodInfo?.GetParameters();

            if (actionArguments != null)
            {
                foreach (var actionArgument in actionArguments)
                {
                    var modelType = actionArgument.ParameterType;

                    if (IsToValidate(modelType))
                    {
                        if (_validators.TryGetValue(modelType, out var validator) && validator is IValidator modelValidator)
                        {
                            context.Request.EnableBuffering();
                            var initialPosition = context.Request.Body.Position;
                            var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
                            var model = JsonConvert.DeserializeObject(json, modelType);

                            var validationContext = new ValidationContext<object>(model!);
                            var validationResult = await modelValidator.ValidateAsync(validationContext);

                            if (!validationResult.IsValid)
                            {
                                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                                var result = new JsonResult(new Dictionary<string, List<string>> { { "errors", errorMessages } })
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    ContentType = "application/json"
                                };
                                await result.ExecuteResultAsync(new ActionContext { HttpContext = context });
                                return;
                            }
                            context.Request.Body.Seek(initialPosition, SeekOrigin.Begin);
                        }
                    }
                }
            }

            await _next(context);
        }
        private bool IsToValidate(Type modelType)
        {
            return 
                modelType == typeof(AccessTokenDTO) || 
                modelType == typeof(RefreshTokenDTO) || 
                modelType == typeof(UserLoginDTO) || 
                modelType == typeof(UserRegisterDTO);
        }

    }
}
