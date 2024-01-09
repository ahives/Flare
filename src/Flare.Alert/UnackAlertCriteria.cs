namespace Flare.Alert;

public interface UnackAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}