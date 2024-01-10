namespace Flare.Alert;

using Flare.Model;
using Model;

public interface Alert :
    FlareAPI
{
    Task<Maybe<CreateAlertInfo>> Create(Action<CreateAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<DeleteAlertInfo>> Delete(string identifier, IdentifierType identifierType,
        Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertStatusInfo>> Status(Guid requestId, CancellationToken cancellationToken = default);

    Task<Maybe<AlertInfo>> Get(string identifier, IdentifierType identifierType,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AckAlertInfo>> Acknowledge(string identifier, IdentifierType identifierType,
        Action<AckAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<UnackAlertInfo>> Unacknowledge(string identifier, IdentifierType identifierType,
        Action<UnackAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<EscalateAlertInfo>> Escalate(string identifier, IdentifierType identifierType,
        Action<EscalateAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AssignAlertInfo>> Assign(string identifier, IdentifierType identifierType,
        Action<AssignAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<SnoozeAlertInfo>> Snooze(string identifier, IdentifierType identifierType,
        Action<SnoozeAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertCloseInfo>> Close(string identifier, IdentifierType identifierType, Action<CloseAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertNoteInfo>> AddNote(string identifier, IdentifierType identifierType,
        Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default);
}