namespace Flare.Alert;

public interface EscalationAlertIdentifier
{
    void Id(Guid identifier);

    void Name(string name);
}