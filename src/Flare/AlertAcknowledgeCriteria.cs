namespace Flare;

public interface AlertAcknowledgeCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);

    void SearchIdentifierType(AcknowledgeSearchIdentifierType type);
}