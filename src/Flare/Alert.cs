using Flare.API.Model;

namespace Flare;

public interface Alert :
    FlareAPI
{
    Task<Result> Create(Action<CreateAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertStatusData>> GetStatus(Guid requestId, CancellationToken cancellationToken = default);

    Task<Result<AlertData>> Get(Guid identifier, Action<GetAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AbbreviatedAlertData>> List(Action<ListAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result<AlertCountData>> Count(Action<CountAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> Delete(Guid identifier, Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> Acknowledge(Guid identifier, Action<AcknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Result> Close(Action<CloseAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Result> AddNote(Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default);
}