using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace wedev.Domain.Global;

public class DeploymentEnvironment
{
    public Guid EnvironmentId { get; set; } = Guid.NewGuid();
    [MaxLength(255)]
    public string Name { get; set; } = null!; // z.B. "Production", "Staging"

    // Verknüpfte Server und Datenbanken in dieser Umgebung
    public ICollection<Server> Servers { get; set; } = new List<Server>();
    public ICollection<Database> Databases { get; set; } = new List<Database>();
}