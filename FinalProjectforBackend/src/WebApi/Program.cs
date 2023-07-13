using Application;
using Application.Common.Interfaces;
using Domain.Settings;
using Infrastructure;
using WebApi.Hubs;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);

builder.Services.AddScoped<IOrderHubService, OrderHubManager>();

builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();


app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.MapHub<SeleniumLogHub>("/Hubs/SeleniumLogHub");

app.Run();

