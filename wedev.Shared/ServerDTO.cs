namespace wedev.Shared;

public class ServerDTO
{
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string AdditionalParams { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid EnvironmentId { get; set; }
    public DeploymentEnvironmentDTO DeploymentEnvironment { get; set; }
    public List<DatabaseDTO> Databases { get; set; } = new List<DatabaseDTO>();
}