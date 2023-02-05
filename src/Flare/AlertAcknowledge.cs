namespace Flare;

public interface AlertAcknowledge
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}