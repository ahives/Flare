namespace Flare.Alert;

public interface SnoozeAlertCriteria
{
    void EndTime(DateTimeOffset? endTime);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}