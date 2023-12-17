namespace Flare;

public interface DeleteAlertCriteria
{
    void SearchIdentifierType(DeleteSearchIdentifierType type);

    void User(string displayName);

    void Source(string displayName);
}