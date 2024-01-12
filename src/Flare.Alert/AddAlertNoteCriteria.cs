namespace Flare.Alert;

public interface AddAlertNoteCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}