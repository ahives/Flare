namespace Flare;

public interface AlertClosureCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);

    void SearchIdentifierType(CloseSearchIdentifierType type);
}