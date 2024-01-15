namespace Flare.Alert;

public interface AddAlertResponderCriteria
{
    void Responder(Action<AlertResponderIdentifier> action);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}