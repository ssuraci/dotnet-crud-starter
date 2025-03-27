using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCrudStarter.Controller;
using NetCrudStarter.StudentModule.Dto;
using NetCrudStarter.StudentModule.Entities;
using NetCrudStarter.StudentModule.Entities.filters;
using NetCrudStarter.StudentModule.Service;
using NetCrudStarter.StudentModule.Validator;
using NetCrudStarter.Service;

namespace NetCrudStarter.StudentModule.Controllers;


[Route("api/[controller]")
]
public class StudentController:  BaseController<Student, StudentDto, StudentValidator, int, StudentFilter>
{
    protected StudentService service;
    
    public StudentController(ILogger<Student> logger, IMapper mapper, StudentValidator validator, StudentService StudentService) : base(logger, mapper, validator)
    {
        service = StudentService;
    }

    protected override BaseService<Student, int> GetBaseService()
    {
       return service;
    }
}