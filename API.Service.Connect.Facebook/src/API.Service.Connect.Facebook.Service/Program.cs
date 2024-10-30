using API.Service.Connect.Facebook.Domain.Implementation.Interfaces;
using API.Service.Connect.Facebook.Domain.Implementation.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAnyOrigin",
		policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
	c.EnableAnnotations();

	c.SwaggerDoc("v1", new OpenApiInfo { Title = "API - Service Connect Facebook", Version = "v1" });

	c.TagActionsBy(api =>
	{
		if (api.GroupName != null)
		{
			return new[] { api.GroupName };
		}

		if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
		{
			return new[] { controllerActionDescriptor.ControllerName };
		}

		throw new InvalidOperationException("Unable to determine tag for endpoint.");
	});
	c.DocInclusionPredicate((name, api) => true);
});

builder.Services.AddScoped<IFacebookService, FacebookService>();

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();