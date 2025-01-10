using System;
using System.Collections.Generic;
using NetCrudStarter.Model;

namespace NetCrudStarter.Demo.Entities;

public partial class Course : BaseEntity<int>
{

    public string? Description { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? StartDate { get; set; }

    public string? Title { get; set; }

    public int? TeacherId { get; set; }

    public virtual ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();

    public virtual Teacher? Teacher { get; set; }
}
