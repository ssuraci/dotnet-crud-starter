using System;
using System.Collections.Generic;
using NetCrudStarter.Model;

namespace NetCrudStarter.Demo.Entities;

public partial class Teacher: BaseEntity<int>
{
    public DateOnly? BirthDate { get; set; }

    public string? Category { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? SchoolId { get; set; }

    public int? SubjectId { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual School? School { get; set; }

    public virtual Subject? Subject { get; set; }
}
