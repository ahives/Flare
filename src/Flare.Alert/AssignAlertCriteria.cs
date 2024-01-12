namespace Flare.Alert;

public interface AssignAlertCriteria
{
    void Owner(Action<AssignAlertIdentifier> action);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}