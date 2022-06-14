using System.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MvcDemo.WebSite.Infrastructure
{
    public class TransactionAttribute: ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var transaction = context.HttpContext.RequestServices.GetRequiredService<IDbTransaction>();
            await base.OnActionExecutionAsync(context, next);
            transaction.Commit();
        }
    }
}
