namespace Flare;

public interface AlertDeleteQuery
{
    // void Query(string query);

    void SearchIdentifierType(DeleteSearchIdentifierType type);

    void User(string displayName);

    void Source(string displayName);
}