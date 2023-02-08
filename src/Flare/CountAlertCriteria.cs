namespace Flare;

public interface CountAlertCriteria
{
    // void Query(string query);

    void SearchIdentifier(Guid searchIdentifier);

    void SearchIdentifierType(QuerySearchIdentifierType type);
}