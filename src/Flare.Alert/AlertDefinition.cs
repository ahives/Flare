using Flare.Model;

namespace Flare.Alert;

public interface AlertDefinition
{
    void Description(string description);

    void Responders(Action<Responder> action);

    void VisibleTo(Action<VisibleTo> action);

    void ClientIdentifier(string alias);

    void AdditionalNotes(string notes);

    void CustomProperties(Action<AlertProperty> action);

    void CustomActions(string action, params string[] actions);
    
    void CustomTags(string tag, params string[] tags);

    void DomainRelatedTo(string entity);

    void Priority(AlertPriority priority);
}