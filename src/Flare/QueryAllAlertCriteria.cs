namespace Flare;

public interface QueryAllAlertCriteria
{
    void SearchIdentifier(Guid identifier);

    void SearchIdentifierType(IdentifierType type);

    void Offset(int offset);

    void Limit(int limit);

    void Sort(SortableFields field);

    void Order(OrderType type);
}