namespace Flare.Alert;

public interface CloseAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}