using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Lr5
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorLoggingMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var myCookie = context.Request.Cookies["MyCookie"];
                    if (string.IsNullOrEmpty(myCookie))
                    {
                        await context.Response.WriteAsync("Cookie is not set.");
                    }
                    else
                    {
                        await context.Response.WriteAsync($"Cookie value: {myCookie}");
                    }
                });

                endpoints.MapPost("/", async context =>
                {
                    var value = context.Request.Form["value"];
                    var expiry = DateTime.Parse(context.Request.Form["expiry"]);

                    context.Response.Cookies.Append("MyCookie", value, new CookieOptions
                    {
                        Expires = expiry
                    });

                    await context.Response.WriteAsync("Data has been stored in cookie.");
                });
            });
        }
    }
}