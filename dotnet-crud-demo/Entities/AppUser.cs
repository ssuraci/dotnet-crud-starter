﻿using System;
using System.Collections.Generic;
using NetCrudStarter.Model;

namespace NetCrudStarter.Demo.Entities;

public partial class AppUser: BaseEntity<int>
{
    
    public string? FirstName { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? LastName { get; set; }

    public string? Passwd { get; set; }

    public string? Username { get; set; }
}
