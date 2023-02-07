namespace Flare;

public interface IFlareClient
{
    T API<T>() where T : FlareAPI;
}