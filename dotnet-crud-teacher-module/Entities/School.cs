using System;
using System.Collections.Generic;
using NetCrudStarter.Model;

namespace NetCrudStarter.TeacherModule.Entities;

public partial class School  : BaseEntity<int>
{
    public string? Category { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public double? Lat { get; set; }

    public double? Lng { get; set; }
    
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
