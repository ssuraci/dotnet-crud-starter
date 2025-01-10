using NetCrudStarter.Demo.Dto;
using NetCrudStarter.Demo.Entities;

namespace NetCrudStarter.Demo.Mapper;

public class TeacherMapper: AutoMapper.Profile
{
    public TeacherMapper()
    {
        CreateMap<Teacher, TeacherDto>().ForMember(dest => dest.SchoolName, 
            source => source.MapFrom(src => src.School!.Name));

        CreateMap<TeacherDto, Teacher>();
        ;
    }
}