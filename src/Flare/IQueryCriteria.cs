namespace Flare;

using Model;

public interface IQueryCriteria
{
    Dictionary<string, QueryArg> GetQueryArguments();
}