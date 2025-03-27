using NetCrudStarter.TeacherModule.Dto;
using NetCrudStarter.TeacherModule.Entities;

namespace NetCrudStarter.TeacherModule.Mapper;

public class TeacherMapper: AutoMapper.Profile
{
    public TeacherMapper()
    {
        CreateMap<Teacher, TeacherDto>().ForMember(dest => dest.SchoolName, 
            source => source.MapFrom(src => src.School!.Name));

        CreateMap<TeacherDto, Teacher>();
    }
}
