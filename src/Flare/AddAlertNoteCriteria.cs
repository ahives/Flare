namespace Flare;

public interface AddAlertNoteCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}