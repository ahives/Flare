namespace Flare.Alert;

public interface AddAlertTeamCriteria
{
    void Team(Action<AlertTeamIdentifier> action);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}