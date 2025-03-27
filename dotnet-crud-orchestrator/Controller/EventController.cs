using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using NetCrudStarter.TeacherModule.Dto;

namespace NetCrudStarter.OrchestratorModule.Service;

public class EventController : ControllerBase
{
    protected ILogger<EventController> Logger { get; }
    public EventController(ILogger<EventController> logger)
    {
        Logger = logger;
    }

    [NonAction]
    [CapSubscribe("entity.created.Teacher")]
    public void OnTeacherCreate(TeacherDto teacherDto)
    {
        Logger.LogInformation("[EVENTBUS] teacher created: " + teacherDto.LastName);
    }

    [NonAction]
    [CapSubscribe("entity.modified.Teacher")]
    public void OnTeacherUpdate(TeacherDto teacherDto)
    {
        Logger.LogInformation("[EVENTBUS] teacher updated: " + teacherDto.LastName);
    }

}