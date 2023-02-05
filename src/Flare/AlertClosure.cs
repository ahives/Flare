namespace Flare;

public interface AlertClosure
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}