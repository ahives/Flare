namespace Flare.Configuration;

public interface IFlareConfigProvider
{
    FlareConfig Configure(Action<FlareConfigurator> configurator);

    FlareConfig Configure(string file);
}