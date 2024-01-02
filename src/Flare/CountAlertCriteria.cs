namespace Flare;

public interface CountAlertCriteria
{
    void SearchIdentifier(Guid identifier);

    void SearchIdentifierType(IdentifierType type);
}