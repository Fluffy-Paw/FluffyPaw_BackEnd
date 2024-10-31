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
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
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
app.UseCors("CorsPolicy");

//Middleware
app.UseRouting();

app.UseAuthentication();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();


app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/NotificationHub");
});


app.Run();
