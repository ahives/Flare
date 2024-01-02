namespace Flare;

public interface CloseAlertQuery
{
    void SearchIdentifier(Guid identifier);
    
    void SearchIdentifierType(IdentifierType type);
}