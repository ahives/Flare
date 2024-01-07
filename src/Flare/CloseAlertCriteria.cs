namespace Flare;

public interface CloseAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}