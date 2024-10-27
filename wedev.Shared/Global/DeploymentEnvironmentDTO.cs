using System;
using System.Collections.Generic;

namespace wedev.Shared.Global;

public class DeploymentEnvironmentDto
{
    public Guid EnvironmentId { get; set; }
    public string Name { get; set; } = null!; // z.B. "Production", "Staging"
}