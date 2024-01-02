namespace Flare;

using Flare.API.Model;

public interface Alert :
    FlareAPI
{
    Task<Maybe<AlertResponse>> Create(Action<CreateAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertResponse>> Delete(Guid identifier, IdentifierType identifierType,
        Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertStatusInfo>> Status(Guid requestId, CancellationToken cancellationToken = default);

    Task<Maybe<AlertInfo>> Get(string identifier, IdentifierType identifierType, CancellationToken cancellationToken = default);

    Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AcknowledgeInfo>> Acknowledge(Guid identifier, IdentifierType identifierType, Action<AcknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertCloseInfo>> Close(Action<CloseAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertNoteInfo>> AddNote(Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default);
}