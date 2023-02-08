namespace Flare;

public interface AlertCountQueryCriteria
{
    // void Query(string query);

    void SearchIdentifier(Guid searchIdentifier);

    void SearchIdentifierType(QuerySearchIdentifierType type);
}