namespace Flare;

public interface AlertCountQuery
{
    // void Query(string query);

    void SearchIdentifier(Guid searchIdentifier);

    void SearchIdentifierType(QuerySearchIdentifierType type);
}