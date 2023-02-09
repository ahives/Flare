namespace Flare;

public interface AddAlertNoteCriteria
{
    void Definition(Action<AlertNoteDefinition> definition);
    
    void Where(Action<AddAlertNoteQuery> query);
}