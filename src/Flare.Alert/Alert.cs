namespace Flare.Alert;

using Flare.Model;
using Model;

public interface Alert :
    FlareAPI
{
    Task<Maybe<ResultInfo>> Create(Action<CreateAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Delete(string identifier, IdentifierType identifierType,
        Action<DeleteAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<AlertStatusInfo>> Status(Guid requestId, CancellationToken cancellationToken = default);

    Task<Maybe<AlertInfo>> Get(string identifier, IdentifierType identifierType,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertAllInfo>> GetAll(Action<QueryAllAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<AlertCountInfo>> Count(Action<CountAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Acknowledge(string identifier, IdentifierType identifierType,
        Action<AckAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Unacknowledge(string identifier, IdentifierType identifierType,
        Action<UnackAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Escalate(string identifier, IdentifierType identifierType,
        Action<EscalateAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Assign(string identifier, IdentifierType identifierType,
        Action<AssignAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Snooze(string identifier, IdentifierType identifierType,
        Action<SnoozeAlertCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> Close(string identifier, IdentifierType identifierType, Action<CloseAlertCriteria> criteria,
        CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> AddTeam(string identifier, IdentifierType identifierType,
        Action<AddAlertTeamCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> AddResponder(string identifier, IdentifierType identifierType,
        Action<AddAlertResponderCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> AddTags(string identifier, IdentifierType identifierType,
        Action<AddAlertTagsCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> AddCustomProperties(string identifier, IdentifierType identifierType,
        Action<AddAlertCustomPropertiesCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> DeleteCustomProperties(string identifier, IdentifierType identifierType,
        Action<DeleteAlertCustomPropertiesCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> UpdatePriority(string identifier, IdentifierType identifierType,
        AlertPriority priority, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> AddNote(string identifier, IdentifierType identifierType,
        Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default);

    Task<Maybe<ResultInfo>> DeleteTags(string identifier, IdentifierType identifierType,
        Action<DeleteAlertTagsCriteria> criteria, CancellationToken cancellationToken = default);
}