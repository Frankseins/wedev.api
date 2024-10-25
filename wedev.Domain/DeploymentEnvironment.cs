using System;
using System.Collections.Generic;

namespace wedev.Domain;

public class DeploymentEnvironment
{
    public Guid EnvironmentId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } // z.B. "Production", "Staging"

    // Verknüpfte Server und Datenbanken in dieser Umgebung
    public ICollection<Server> Servers { get; set; } = new List<Server>();
    public ICollection<Database> Databases { get; set; } = new List<Database>();
}