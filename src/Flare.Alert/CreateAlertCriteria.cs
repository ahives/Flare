namespace Flare.Alert;

using Model;

public interface CreateAlertCriteria
{
    void Description(string description);

    void Responders(Action<AddResponder> action);

    void VisibleTo(Action<VisibleTo> action);

    void Alias(string alias);

    void Source(string source);

    void User(string user);

    void AdditionalNotes(string notes);

    void CustomProperties(Action<AlertProperty> action);

    void CustomActions(string action, params string[] actions);
    
    void Tags(Action<TagBuilder> action);

    void RelatedToDomain(string entity);

    void Priority(AlertPriority priority);

    void Message(string message);
}