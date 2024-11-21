using FluffyPaw_Infrastructure.DependencyInjection;
using FluffyPaw_API.Middleware;
using Microsoft.OpenApi.Models;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Infrastructure.Intergrations.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Build CORS
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000", 
            "http://192.168.2.3:3000",
            "http://127.0.0.1:5500")
            //.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "FluffyPaw_API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
builder.Services.AddHttpContextAccessor();
// Add custom services and dependencies
builder.Services.InfrastructureService(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FluffyPaw_API");
        c.RoutePrefix = "";
        c.EnableTryItOutByDefault();
    });
}


app.UseHttpsRedirection();


//Middleware
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/NotificationHub").RequireCors("CorsPolicy");
});


app.Run();
