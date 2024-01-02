namespace Flare;

public interface AddAlertNoteQuery
{
    void SearchIdentifier(Guid identifier);
    
    void SearchIdentifierType(IdentifierType type);
}