using AutoMapper;
using NetCrudStarter.TeacherModule.Dto;
using NetCrudStarter.TeacherModule.Entities;

namespace NetCrudStarter.TeacherModule.Mapper;

public class CourseMapper : Profile
{
    public CourseMapper()
    {
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.TeacherName,
                source => source.MapFrom(src => src.Teacher!.LastName));
        
        CreateMap<CourseDto, Course>();
    }
}
