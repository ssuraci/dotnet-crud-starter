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
public class CourseController: BaseController<Course, CourseDto, CourseValidator, int, CourseFilter>
{
    protected CourseService service;
    
    public CourseController(ILogger<Course> logger, IMapper mapper, CourseValidator validator, CourseService courseService) : base(logger, mapper, validator)
    {
        service = courseService;
    }

    protected override BaseService<Course, int> GetBaseService()
    {
       return service;
    }
}
