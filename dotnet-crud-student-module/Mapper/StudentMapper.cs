using NetCrudStarter.StudentModule.Dto;
using NetCrudStarter.StudentModule.Entities;

namespace NetCrudStarter.StudentModule.Mapper;

public class StudentMapper: AutoMapper.Profile
{
    public StudentMapper()
    {
        CreateMap<Student, StudentDto>();
        CreateMap<StudentDto, Student>();
        ;
    }
}