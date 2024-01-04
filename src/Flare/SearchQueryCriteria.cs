namespace Flare;

using API.Model;

public interface SearchQueryCriteria :
    IQueryCriteria
{
    void Status(AlertStatus status);
}