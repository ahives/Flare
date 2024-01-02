namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertCloseInfo>> Close(Action<CloseAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new CloseAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/close"
            : $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/close?{queryString}";

        if (impl.Errors.Any())
            return Response.Failed<AlertCloseInfo>(Debug.WithErrors(url, impl.Errors));

        return await PostRequest<AlertCloseInfo, CloseAlertRequest>(url, impl.Request, cancellationToken);
    }


    class CloseAlertCriteriaImpl :
        CloseAlertCriteria
    {
        public Guid Identifier { get; private set; }
        public IDictionary<string, object> QueryArguments { get; private set; }
        public CloseAlertRequest Request { get; private set; }
        public List<Error> Errors { get; private set; }

        public void Definition(Action<CloseAlertDefinition> definition)
        {
            var impl = new CloseAlertDefinitionImpl();
            definition?.Invoke(impl);

            Request = impl.Request;
        }

        public void Where(Action<CloseAlertQuery> query)
        {
            var impl = new CloseAlertQueryImpl();
            query?.Invoke(impl);

            Identifier = impl.QueryIdentifier;
            QueryArguments = impl.QueryArguments;
            Errors = impl.Errors;
        }


        class CloseAlertDefinitionImpl :
            CloseAlertDefinition
        {
            string _note;
            string _source;
            string _user;

            public CloseAlertRequest Request =>
                new()
                {
                    Note = _note,
                    Source = _source,
                    User = _user
                };

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


        class CloseAlertQueryImpl :
            CloseAlertQuery
        {
            public Guid QueryIdentifier { get; private set; }
            public IDictionary<string, object> QueryArguments { get; }
            public List<Error> Errors { get; private set; }

            public CloseAlertQueryImpl()
            {
                QueryArguments = new Dictionary<string, object>();
                Errors = new List<Error>();
            }

            public void SearchIdentifier(Guid identifier)
            {
                QueryIdentifier = identifier;
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
}