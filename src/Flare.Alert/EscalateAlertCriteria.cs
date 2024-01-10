namespace Flare.Alert;

public interface EscalateAlertCriteria
{
    void Escalation(Action<EscalationAlertIdentifier> action);

    void User(string displayName);

    void Source(string displayName);

    void Note(string note);
}