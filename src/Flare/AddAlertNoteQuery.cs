namespace Flare;

public interface AddAlertNoteQuery
{
    void Identifier(Guid identifier);
    
    void IdentifierType(AddAlertNoteIdentifierType type);
}