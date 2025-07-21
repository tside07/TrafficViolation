using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class ReportStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ViolationReport> ViolationReports { get; set; } = new List<ViolationReport>();
}
