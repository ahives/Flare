namespace Flare;

using Flare.API.Model;

public interface Alert :
    FlareAPI
{
    Task<Maybe<CreateAlertInfo>> Create(Action<CreateAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<DeleteAlertInfo>> Delete(Guid identifier, IdentifierType identifierType,
        Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertStatusInfo>> Status(Guid requestId, CancellationToken cancellationToken = default);

    Task<Maybe<AlertInfo>> Get(string identifier, IdentifierType identifierType,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AcknowledgeAlertInfo>> Acknowledge(string identifier, IdentifierType identifierType,
        Action<AcknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<UnacknowledgeAlertInfo>> Unacknowledge(string identifier, IdentifierType identifierType,
        Action<UnacknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<SnoozeAlertInfo>> Snooze(string identifier, IdentifierType identifierType,
        Action<SnoozeAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertCloseInfo>> Close(Action<CloseAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertNoteInfo>> AddNote(string identifier, IdentifierType identifierType,
        Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default);
}