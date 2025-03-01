namespace SellingBook.MiddleWares
{
    public class AttachUserMiddelWare
    {
        private readonly RequestDelegate _next;
        public AttachUserMiddelWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var user = context.Request.Body;
            if (user != null)
            {
                context.Items["User"] = user;
            }
            await _next(context);
        }
    }
}
