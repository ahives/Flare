namespace Flare.Alert;

public interface AddAlertTagsCriteria
{
    void Tags(List<string> tags);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}