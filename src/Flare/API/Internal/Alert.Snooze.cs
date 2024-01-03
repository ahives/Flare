namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<SnoozeAlertInfo>> Snooze(Guid identifier, IdentifierType identifierType, Action<SnoozeAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new SnoozeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var errors = new List<Error>();

        if (impl.Request.EndTime is null)
            errors.Add(new Error{Type = ErrorType.EndTime, Reason = "endTime is a required", Timestamp = DateTimeOffset.UtcNow});

        string idType = GetIdentifierType(identifierType);
        if (string.IsNullOrWhiteSpace(idType))
            errors.Add(new Error{Type = ErrorType.IdentifierType, Reason = "identifierType is required.", Timestamp = DateTimeOffset.UtcNow});

        string url =
            $"https://api.opsgenie.com/v2/alerts/{identifier}/snooze?identifierType={idType}";

        if (errors.Count != 0)
            return Response.Failed<SnoozeAlertInfo>(Debug.WithErrors(url, errors));

        return await PostRequest<SnoozeAlertInfo, SnoozeAlertRequest>(url, impl.Request, cancellationToken);

        string GetIdentifierType(IdentifierType type) =>
            type switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };
    }


    class SnoozeAlertCriteriaImpl :
        SnoozeAlertCriteria
    {
        string _note;
        string _source;
        string _user;
        DateTimeOffset? _endTime;

        public SnoozeAlertRequest Request =>
            new()
            {
                EndTime = _endTime,
                Note = _note,
                Source = _source,
                User = _user
            };

        public void EndTime(DateTimeOffset? endTime)
        {
            _endTime = endTime;
        }

        public void User(string displayName)
        {
            _user = displayName;
        }

        public void Source(string displayName)
        {
            _source = displayName;
        }

        public void Note(string note)
        {
            _note = note;
        }
    }
}