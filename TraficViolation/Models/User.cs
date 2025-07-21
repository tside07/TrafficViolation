using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class User
{
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual Citizen? Citizen { get; set; }

    public virtual UserRole Role { get; set; } = null!;
}
