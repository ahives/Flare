namespace Flare.Alert;

public interface UnacknowledgeAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}