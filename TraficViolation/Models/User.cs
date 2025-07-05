using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class User
{
    public long Id { get; set; }

    public long? CitizenId { get; set; }

    public long RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual Citizen? Citizen { get; set; }

    public virtual UserRole Role { get; set; } = null!;
}
