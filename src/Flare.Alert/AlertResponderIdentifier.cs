namespace Flare.Alert;

using Flare.Model;

public interface AlertResponderIdentifier
{
    void Id(Guid id);

    void Type(ResponderType responder);

    void Username(string username);
}