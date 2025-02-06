using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCrudStarter.Controller;
using NetCrudStarter.Demo.Dto;
using NetCrudStarter.Demo.Entities;
using NetCrudStarter.Demo.Entities.filters;
using NetCrudStarter.Demo.Service;
using NetCrudStarter.Demo.Validator;
using NetCrudStarter.Service;

namespace NetCrudStarter.Demo.Controllers;


[Route("api/[controller]")]
public class TeacherController:  BaseController<Teacher, TeacherDto, TeacherValidator, int, TeacherFilter>
{
    protected TeacherService service;
    public TeacherController(ILogger<Teacher> logger, IMapper mapper, TeacherValidator validator, TeacherService teacherService) : base(logger, mapper, validator)
    {
        service = teacherService;
    }

    protected override BaseService<Teacher, int> GetBaseService()
    {
       return service;
    }
}