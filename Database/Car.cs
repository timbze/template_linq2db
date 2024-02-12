namespace TemplateLinq2DbFastEndpoints.Database;

public sealed class Car
{
    public Guid Id { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
}