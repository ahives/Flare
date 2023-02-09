namespace Flare;

public interface AlertNoteDefinition
{
    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}