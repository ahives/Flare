namespace Flare;

public interface AlertListQuery
{
    // void Query(string query);
    
    void SearchIdentifier(Guid searchIdentifier);

    void SearchIdentifierType(QuerySearchIdentifierType type);

    void Offset(int offset);

    void Limit(int limit);

    void Sort(SortField field);

    void Order(OrderType type);
}