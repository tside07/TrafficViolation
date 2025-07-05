using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class ViolationReport
{
    public long Id { get; set; }

    public long CitizenId { get; set; }

    public long VehicleId { get; set; }

    public long ViolationTypeId { get; set; }

    public long StatusId { get; set; }

    public DateTimeOffset? ReportDate { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public virtual Citizen Citizen { get; set; } = null!;

    public virtual ReportStatus Status { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;

    public virtual ViolationType ViolationType { get; set; } = null!;
}
