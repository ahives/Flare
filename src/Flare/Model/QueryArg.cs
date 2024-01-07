namespace Flare.Model;

public sealed record QueryArg
{
    public bool IsSearchQuery { get; init; }
    
    public object Value { get; init; }
}