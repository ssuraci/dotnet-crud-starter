using System;
using System.Collections.Generic;
using NetCrudStarter.Model;

namespace NetCrudStarter.Demo.Entities;

public partial class Student : BaseEntity<int>
{
    
    public DateTime? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? SchoolId { get; set; }

    public virtual ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    public virtual School? School { get; set; }
}
