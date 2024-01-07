namespace Flare.API.Internal;

using System.Collections.Immutable;
using Model;

public partial class AlertImpl
{
    public async Task<Maybe<SnoozeAlertInfo>> Snooze(string identifier, IdentifierType identifierType, Action<SnoozeAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new SnoozeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = new List<Error>();
        errors.AddRange(Validate());
        errors.AddRange(qc.Validate());

        if (errors.Count != 0)
            return Response.Failed<SnoozeAlertInfo>(Debug.WithErrors("alerts/{identifier}/snooze?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/snooze?identifierType={GetIdentifierType()}";

        return await PostRequest<SnoozeAlertInfo, SnoozeAlertRequest>(url, impl.Request, cancellationToken);

        string GetIdentifierType() =>
            identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };

        IReadOnlyList<Error> Validate()
        {
            bool isIdentifierTypeMissing = identifierType switch
            {
                IdentifierType.Id => false,
                IdentifierType.Tiny => false,
                IdentifierType.Alias => false,
                _ => true
            };

            var errors = new List<Error>();

            if (isIdentifierTypeMissing)
                errors.Add(Errors.Create(ErrorType.IdentifierType,
                    $"{identifierType.ToString()} is not a valid identifier type in the current context."));

            bool isGuid = Guid.TryParse(identifier, out _);

            if (isGuid && identifierType != IdentifierType.Id)
                errors.Add(Errors.Create(ErrorType.IdentifierType,
                    "Identifier type is not compatible with identifier."));

            if (isGuid && identifier != default && isIdentifierTypeMissing ||
                (string.IsNullOrWhiteSpace(identifier) && isIdentifierTypeMissing))
                errors.Add(Errors.Create(ErrorType.IdentifierType, "Identifier type is missing."));

            return errors;
        }
    }


    class SnoozeAlertCriteriaImpl :
        SnoozeAlertCriteria,
        IQueryCriteria
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

        public bool IsSearchQuery() => false;

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();
            if (!_endTime.HasValue)
                errors.Add(Errors.Create(ErrorType.EndTime, "EndTime is required."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);
    }
}