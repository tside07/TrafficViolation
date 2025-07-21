using System;
using System.Collections.Generic;

namespace TraficViolation.Models;

public partial class ViolationReport
{
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public int VehicleId { get; set; }

    public int ViolationTypeId { get; set; }

    public int StatusId { get; set; }

    public DateTimeOffset? ReportDate { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public virtual Citizen Citizen { get; set; } = null!;

    public virtual ReportStatus Status { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;

    public virtual ViolationType ViolationType { get; set; } = null!;
}
