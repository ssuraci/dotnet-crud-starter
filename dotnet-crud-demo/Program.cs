using AutoMapper;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using NetCrudStarter.Demo.Context;
using NetCrudStarter.Demo.Mapper;
using NetCrudStarter.Demo.Repo;
using NetCrudStarter.Demo.Service;
using NetCrudStarter.Demo.Validator;

using NetCrudStarter.Middleware;


var builder = WebApplication.CreateBuilder(args);

var proxyGenerator = new ProxyGenerator();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DemoDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<DbContext, DemoDbContext>();
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<TeacherService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<TeacherService>>();
    var dbContext = provider.GetRequiredService<DemoDbContext>();
    var teacherRepo = provider.GetRequiredService<TeacherRepository>();
    var service = new TeacherService(logger, teacherRepo);
    return (TeacherService) proxyGenerator.CreateClassProxy(typeof(TeacherService), 
        ProxyGenerationOptions.Default, 
        new object[] {logger, teacherRepo}, 
        new TransactionInterceptor(dbContext, provider.GetRequiredService<ILogger<TransactionInterceptor>>()));
});

builder.Services.AddScoped<TeacherMapper>();
builder.Services.AddScoped<TeacherValidator>();
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new TeacherMapper());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

public partial class Program { } 
