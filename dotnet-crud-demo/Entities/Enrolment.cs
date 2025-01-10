using System;
using System.Collections.Generic;
using NetCrudStarter.Model;

namespace NetCrudStarter.Demo.Entities;

public partial class Enrolment : BaseEntity<int>
{
    
    public DateTime? EnrolmentDate { get; set; }

    public int? CourseId { get; set; }

    public int? StudentId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Student { get; set; }
}
