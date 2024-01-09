namespace Flare.Alert;

public interface AckAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}