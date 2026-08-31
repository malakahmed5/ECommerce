using ECommerce.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Attributes
{
    public class RedisCacheAttribute: ActionFilterAttribute
    {
        private readonly int _timeInMinutes;

        public RedisCacheAttribute(int timeInMinutes)
        {
            _timeInMinutes = timeInMinutes;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Get Service From DI Container 
            var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            //Create Key
            var cacheKey = CreateKey(context.HttpContext.Request);

            //Check If Data Exist in Redis [Get] ?
            var cacheValue = await _cacheService.GetDataAsync(cacheKey);
            //If Exist => [Skip Calling EndPoint] + Return Data From Cache [New Content Result]
            if(cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "applications/Json"
                };
                return;
            }
            //If Doesn't Exist => [Calling EndPoint] + [if Response = 200 Ok Store Data In Cache [Set]]
            var ExecutionContext = await next.Invoke();
            if (ExecutionContext.Result is OkObjectResult result)
                await _cacheService.SetDataAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(_timeInMinutes));

        }
        private string CreateKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(x => x.Key)) //api/products|barndId-2|typeId-3
                Key.Append($"|{item.Key}-{item.Value}");
            return Key.ToString();
        }

    }
}
