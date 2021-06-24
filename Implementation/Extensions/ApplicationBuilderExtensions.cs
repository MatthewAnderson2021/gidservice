using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mime;

namespace GudelIdService.Implementation.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    switch (exceptionHandlerPathFeature.Error)
                    {
                        default:
                            var text = env.IsDevelopment()
                                ? exceptionHandlerPathFeature.Error.ToString()
                                : string.Empty;
                            await context.Response.WriteAsync(text).ConfigureAwait(false);
                            break;
                    }
                });
            });
        }
    }
}
