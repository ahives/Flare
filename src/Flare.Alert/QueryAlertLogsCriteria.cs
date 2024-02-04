namespace Flare.Alert;

public interface QueryAlertLogsCriteria
{
    void PaginationOffset(int offset);

    void PaginationLimit(int limit);

    void Direction(PageDirection direction);

    void OrderBy(OrderType type);
}