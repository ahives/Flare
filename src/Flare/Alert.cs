using Flare.API.Model;

namespace Flare;

public interface Alert :
    FlareAPI
{
    Task<Result> Create(Action<AlertDefinition> action, CancellationToken cancellationToken = default);

    Task<Result> Create(CreateAlertRequest request, CancellationToken cancellationToken = default);

    Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default);

    Task<Result<AlertData>> Get(Guid identifier, Action<AlertGetQuery> query, CancellationToken cancellationToken = default);

    Task<Result<AbbreviatedAlertData>> List(Action<AlertListQuery> query, CancellationToken cancellationToken = default);

    Task<Result<AlertCountData>> Count(Action<AlertCountQuery> query, CancellationToken cancellationToken = default);

    Task<Result> Delete(Guid identifier, Action<AlertDeleteQuery> query, CancellationToken cancellationToken = default);

    Task<Result> Acknowledge(Guid identifier, Action<AlertAcknowledge> action, Action<AlertAcknowledgeQuery> query,
        CancellationToken cancellationToken = default);

    Task<Result> Acknowledge(Guid identifier, AcknowledgeAlertRequest request, Action<AlertAcknowledgeQuery> query,
        CancellationToken cancellationToken = default);
}