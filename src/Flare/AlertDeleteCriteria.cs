namespace Flare;

public interface AlertDeleteCriteria
{
    // void Query(string query);

    void SearchIdentifierType(DeleteSearchIdentifierType type);

    void User(string displayName);

    void Source(string displayName);
}