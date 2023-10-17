namespace TimesheetService.Shared.Infrastructure.Initializer;

public class InitializerOptions
{
    public InitializerOptions()
    {
        Enabled = true;
    }
    
    /// <summary>
    /// Default value is true.
    /// </summary>
    public bool Enabled { get; set; }
}