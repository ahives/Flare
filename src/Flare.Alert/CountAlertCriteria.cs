namespace Flare.Alert;

using Flare.Model;

public interface CountAlertCriteria
{
    void Query(Action<SearchQueryCriteria> criteria);

    void SearchIdentifier(string identifier);

    void SearchIdentifierType(IdentifierType type);
}