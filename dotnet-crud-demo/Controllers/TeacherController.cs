using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetCrudLib.Controller;
using NetCrudLib.Demo.Dto;
using NetCrudLib.Demo.Entities;
using NetCrudLib.Demo.Service;
using NetCrudLib.Demo.Validator;
using NetCrudLib.Service;

namespace NetCrudLib.Demo.Controllers;


[Route("api/[controller]")]
public class TeacherController:  BaseController<Teacher, TeacherDto, TeacherValidator, int>
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