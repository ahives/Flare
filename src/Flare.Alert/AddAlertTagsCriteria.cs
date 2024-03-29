namespace Flare.Alert;

public interface AddAlertTagsCriteria
{
    void Tags(Action<TagBuilder> action);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}