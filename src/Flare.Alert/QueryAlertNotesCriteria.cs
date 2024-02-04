namespace Flare.Alert;

public interface QueryAlertNotesCriteria
{
    void PaginationOffset(int offset);

    void PaginationLimit(int limit);

    void Direction(PageDirection direction);

    void OrderBy(OrderType type);
}