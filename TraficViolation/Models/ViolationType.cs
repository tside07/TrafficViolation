using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class ViolationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ViolationReport> ViolationReports { get; set; } = new List<ViolationReport>();
}
