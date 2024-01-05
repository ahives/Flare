namespace Flare;

public interface CountAlertCriteria
{
    void Query(Action<SearchQueryCriteria> criteria);

    void SearchIdentifier(string identifier);

    void SearchIdentifierType(IdentifierType type);
}