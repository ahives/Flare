namespace Flare;

using Model;

public interface IQueryCriteria
{
    bool IsSearchQuery();

    IReadOnlyList<Error> Validate();

    Dictionary<string, QueryArg> GetQueryArguments();
}