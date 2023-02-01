namespace Flare.Alert;

public interface IAlert
{
    Result Create(Action<AlertDefinition> action);
}