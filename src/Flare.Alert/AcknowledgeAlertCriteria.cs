namespace Flare.Alert;

public interface AcknowledgeAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}