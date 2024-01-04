namespace Flare;

public interface IQueryCriteria
{
    bool IsQuery();

    IReadOnlyList<Error> Validate();

    IDictionary<string, object> GetQueryArguments();
}