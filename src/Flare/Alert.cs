using Flare.API.Model;

namespace Flare;

public interface Alert :
    FlareAPI
{
    Task<Result<AlertResponse>> Create(Action<CreateAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertResponse>> Delete(Guid identifier, Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default);

    Task<Result<GetAlertResponse>> Get(string identifier, Action<GetAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<ListAlertResponse>> List(Action<ListAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertCountData>> Count(Action<CountAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> Acknowledge(Guid identifier, Action<AcknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Result> Close(Action<CloseAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> AddNote(Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default);
}