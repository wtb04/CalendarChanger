using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CalendarChanger.Attributes
{
    public class AuthorizedAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var expectedKey = config["ApiKey"];
            var session = context.HttpContext.Session;
            var key = session.GetString("ApiKey");

            if (key != expectedKey)
            {
                var path = context.HttpContext.Request.Path.Value ?? "";
                // redirect to login, preserving id if editing
                if (path.Contains("/Edit/", StringComparison.OrdinalIgnoreCase))
                {
                    var id = path.Split('/').Last();
                    context.Result = new RedirectResult($"/EventModifier/Login?id={id}");
                }
                else
                {
                    context.Result = new RedirectResult("/EventModifier/Login");
                }
                return;
            }

            await next();
        }
    }
}