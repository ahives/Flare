namespace Flare.Alert;

public interface AckAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}