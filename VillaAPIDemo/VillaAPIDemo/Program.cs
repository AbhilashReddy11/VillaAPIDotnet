//using Serilog;

using VillaApi;
using VillaApi.Logging;
using Microsoft.EntityFrameworkCore;
using VillaApi.Repository.IRepository;
using VillaApi.Repository;
using VillaAPIDemo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//SQL dependency injection
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});


//register repository in program.cs
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

//Inject Automapper dependency
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().
//WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

// builder.Host.UseSerilog();

builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ILogging, Logging>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();