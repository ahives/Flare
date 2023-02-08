using Flare.API.Model;

namespace Flare;

public interface Alert :
    FlareAPI
{
    Task<Result> Create(Action<AlertDefinitionCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default);

    Task<Result<AlertData>> Get(Guid identifier, Action<AlertGetCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AbbreviatedAlertData>> List(Action<AlertListCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertCountData>> Count(Action<AlertCountQueryCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> Delete(Guid identifier, Action<AlertDeleteCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> Acknowledge(Guid identifier, Action<AlertAcknowledgeCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Result> Close(Guid identifier, Action<AlertClosureCriteria> criteria,
        CancellationToken cancellationToken = default);
}