namespace Flare.Configuration;

public interface FlareConfigurator
{
    void Client(Action<ClientConfigurator> configurator);
}