using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class Vehicle
{
    public long Id { get; set; }

    public long OwnerId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string? Make { get; set; }

    public string? Model { get; set; }

    public int? Year { get; set; }

    public virtual Citizen Owner { get; set; } = null!;

    public virtual ICollection<ViolationReport> ViolationReports { get; set; } = new List<ViolationReport>();
}
