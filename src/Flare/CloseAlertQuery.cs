namespace Flare;

public interface CloseAlertQuery
{
    void Identifier(Guid identifier);
    
    void IdentifierType(CloseSearchIdentifierType type);
}