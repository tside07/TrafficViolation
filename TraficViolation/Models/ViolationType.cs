using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class ViolationType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ViolationReport> ViolationReports { get; set; } = new List<ViolationReport>();
}
