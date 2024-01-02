namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AcknowledgeInfo>> Acknowledge(Guid identifier, IdentifierType identifierType, Action<AcknowledgeAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AcknowledgeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        impl.QueryArguments.Add("identifierType", GetIdentifierType(identifierType));

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";

        if (impl.Errors.Any())
            return Response.Failed<AcknowledgeInfo>(Debug.WithErrors(url, impl.Errors));

        return await PostRequest<AcknowledgeInfo, AcknowledgeAlertRequest>(url, impl.Request, cancellationToken);

        string GetIdentifierType(IdentifierType type) =>
            type switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };
    }


    class AcknowledgeAlertCriteriaImpl :
        AcknowledgeAlertCriteria
    {
        string _note;
        string _source;
        string _user;

        public IDictionary<string, object> QueryArguments { get; }
        public List<Error> Errors { get; private set; }

        public AcknowledgeAlertRequest Request =>
            new()
            {
                Note = _note,
                Source = _source,
                User = _user
            };

        public AcknowledgeAlertCriteriaImpl()
        {
            QueryArguments = new Dictionary<string, object>();
            Errors = new List<Error>();
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

        public void SearchIdentifierType(IdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(searchIdentifierType))
                Errors.Add(new Error{Reason = $"{type.ToString()} is not valid in the current context.", Timestamp = DateTimeOffset.UtcNow});

            QueryArguments.Add("identifierType", searchIdentifierType);
        }
    }
}