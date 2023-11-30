using Aplicacion.Interfaces;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Middlewares
{
    public class OrganizationConnectionMiddleware
    {
        private readonly RequestDelegate _next;
        public OrganizationConnectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var organizationService = context.RequestServices.GetRequiredService<IOrganizationService>();
            var organizationProvider = context.RequestServices.GetRequiredService<IOrganizationProvider>();
            var organizationConfig = context.RequestServices.GetRequiredService<IConfiguration>();
            string slugTenant = context.Request.RouteValues["slugTenant"] as string;

            if(slugTenant == null)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Favor especificar organizacion");
                return;
            }

            var organization = await organizationService.Get(slugTenant);

            if(organization == null)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Favor espeficar una organizacion valida");
                return;
            }

            var connectionString = organizationConfig.GetSection($"Tenants:{slugTenant}").Value;
            organizationProvider.setConnection(connectionString);

             await this._next(context);
        }
    }
}
