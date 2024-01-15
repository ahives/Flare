namespace Flare.Alert;

using Flare.Model;

public interface AlertResponderIdentifier
{
    void Id(Guid id);

    void Type(RecipientType responder);

    void Username(string username);
}