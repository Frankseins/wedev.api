namespace wedev.Shared.Global;

public class ServerDto
{
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string AdditionalParams { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid EnvironmentId { get; set; }
    public DeploymentEnvironmentDto DeploymentEnvironment { get; set; }
    public List<DatabaseDto> Databases { get; set; } = new List<DatabaseDto>();
    
    public string BuildConnectionString()
    {
        var connectionString = $"Host={Host};Port={Port};{AdditionalParams}";
        if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            connectionString += $";Username={Username};Password={Password}";
        }
        return connectionString;
    }
}