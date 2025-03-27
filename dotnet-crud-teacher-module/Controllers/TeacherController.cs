using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCrudStarter.Controller;
using NetCrudStarter.TeacherModule.Dto;
using NetCrudStarter.TeacherModule.Entities;
using NetCrudStarter.TeacherModule.Entities.filters;
using NetCrudStarter.TeacherModule.Service;
using NetCrudStarter.TeacherModule.Validator;
using NetCrudStarter.Service;

namespace NetCrudStarter.TeacherModule.Controllers;


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