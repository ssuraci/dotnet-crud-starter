using System;
using System.Collections.Generic;
using NetCrudLib.Model;

namespace NetCrudLib.Demo.Entities;

public partial class Subject: BaseEntity<int>
{
    
    public string? Code { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
