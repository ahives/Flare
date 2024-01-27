namespace Flare.Model;

public sealed record QueryArg
{
    public bool IsQuery { get; init; }
    
    public object Value { get; init; }
}