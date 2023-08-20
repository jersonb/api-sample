using ExampleToday.Api.Configurations;
using ExampleToday.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;
// Add services to the container.
var CORSPolicy = "CORSPolicy";
services.AddCors(options =>
{
    options.AddPolicy(name: CORSPolicy,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod();
        });
});
services.AddControllers(config =>
{
    config.Conventions.Add(ToKebabParameterTransformer.KebabTransform);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

services.AddSwagger();

services.AddSingleton<FamousPhrases>();

services.AddJwtAuthentication(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CORSPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();