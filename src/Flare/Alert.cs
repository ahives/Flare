using Flare.Model;

namespace Flare;

public interface Alert :
    FlareAPI
{
    Task<Result> Create(Action<AlertDefinition> action, CancellationToken cancellationToken = default);

    Task<Result> Create(CreateAlertRequest request, CancellationToken cancellationToken = default);

    Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default);

    Task<Result<AlertData>> Get(Guid identifier, CancellationToken cancellationToken = default);

    Task<Result<AbbreviatedAlertData>> List(Action<AlertListQuery> query, CancellationToken cancellationToken = default);
}