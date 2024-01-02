namespace Flare;

public interface SnoozeAlertCriteria
{
    void EndTime(DateTimeOffset? endTime);

    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}