using System;
using System.Collections.Generic;

namespace wedev.Shared;

public class DeploymentEnvironmentDTO
{
    public Guid EnvironmentId { get; set; }
    public string Name { get; set; } // z.B. "Production", "Staging"
}