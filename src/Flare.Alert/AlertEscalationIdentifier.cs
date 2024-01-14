namespace Flare.Alert;

public interface AlertEscalationIdentifier
{
    void Id(Guid identifier);

    void Name(string name);
}