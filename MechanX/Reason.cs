namespace MechanX;

public class Reason
{
    public string Message { get; set; } = default!;

    public Dictionary<string, object> Metadata { get; } = new();
}
