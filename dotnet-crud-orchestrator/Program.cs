using AutoMapper;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using NetCrudStarter.StudentModule.Context;
using NetCrudStarter.StudentModule.Mapper;
using NetCrudStarter.StudentModule.Repo;
using NetCrudStarter.StudentModule.Service;
using NetCrudStarter.StudentModule.Validator;

using NetCrudStarter.Middleware;
using NetCrudStarter.OrchestratorModule.Service;
using NetCrudStarter.TeacherModule.Context;
using NetCrudStarter.TeacherModule.Mapper;
using NetCrudStarter.TeacherModule.Repo;
using NetCrudStarter.TeacherModule.Service;
using NetCrudStarter.TeacherModule.Validator;
using Savorboard.CAP.InMemoryMessageQueue;


var builder = WebApplication.CreateBuilder(args);

var proxyGenerator = new ProxyGenerator();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<DbContext, TeacherDbContext>();
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<TeacherMapper>();
builder.Services.AddScoped<TeacherValidator>();
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseMapper>();
builder.Services.AddScoped<CourseValidator>();

builder.Services.AddHttpClient(); // Register HttpClient

builder.Services.AddDbContext<DbContext, StudentDbContext>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<StudentMapper>();
builder.Services.AddScoped<StudentValidator>();

builder.Services.AddScoped<OrchestratorService>();
builder.Services.AddSingleton<EventController>();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new StudentMapper());
    cfg.AddProfile(new TeacherMapper());
    cfg.AddProfile(new CourseMapper());
});


/*
builder.Services.AddScoped<StudentService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<StudentService>>();
    var dbContext = provider.GetRequiredService<DemoDbContext>();
    var StudentRepo = provider.GetRequiredService<StudentRepository>();
    var service = new StudentService(logger, StudentRepo);
    return (StudentService) proxyGenerator.CreateClassProxy(typeof(StudentService),
        ProxyGenerationOptions.Default,
        new object[] {logger, StudentRepo},
        new TransactionInterceptor(dbContext, provider.GetRequiredService<ILogger<TransactionInterceptor>>()));
});
*/



IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCap(x =>
{
    x.UseInMemoryStorage();
    x.UseInMemoryMessageQueue();
});

var app = builder.Build();

ServiceLocator.Provider = app.Services;

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
