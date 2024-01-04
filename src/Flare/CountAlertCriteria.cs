namespace Flare;

public interface CountAlertCriteria :
    IQueryCriteria
{
    void Query(Action<SearchQueryCriteria> criteria);

    void SearchIdentifier(string identifier);

    void SearchIdentifierType(IdentifierType type);
}