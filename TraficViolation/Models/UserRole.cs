﻿using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
