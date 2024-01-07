namespace Flare.Alert;

public interface QueryAllAlertCriteria
{
    void Query(Action<SearchQueryCriteria> criteria);

    void SearchIdentifier(string identifier);

    void SearchIdentifierType(IdentifierType type);

    void PaginationOffset(int offset);

    void PaginationLimit(int limit);

    void SortBy(SortableFields field);

    void OrderBy(OrderType type);
}