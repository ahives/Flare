namespace Flare;

public interface CloseAlertDefinition
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}