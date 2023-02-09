namespace Flare;

public interface CloseAlertCriteria
{
    void Definition(Action<CloseAlertDefinition> definition);
    
    void Where(Action<CloseAlertQuery> query);
}