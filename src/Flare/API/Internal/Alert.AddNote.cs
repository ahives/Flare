namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AlertNoteInfo>> AddNote(Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AddAlertNoteCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/notes"
            : $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/notes?{queryString}";

        if (impl.Errors.Any())
            return Response.Failed<AlertNoteInfo>(Debug.WithErrors(url, impl.Errors));

        return await PostRequest<AlertNoteInfo, AddAlertNoteRequest>(url, impl.Request, cancellationToken);
    }


    class AddAlertNoteCriteriaImpl :
        AddAlertNoteCriteria
    {
        public Guid Identifier { get; private set; }
        public IDictionary<string, object> QueryArguments { get; private set; }
        public AddAlertNoteRequest Request { get; private set; }
        public List<Error> Errors { get; private set; }

        public void Definition(Action<AlertNoteDefinition> definition)
        {
            var impl = new AlertNoteDefinitionImpl();
            definition?.Invoke(impl);

            Request = impl.Request;
        }

        public void Where(Action<AddAlertNoteQuery> query)
        {
            var impl = new AddAlertNoteQueryImpl();
            query?.Invoke(impl);

            Identifier = impl.QueryIdentifier;
            QueryArguments = impl.QueryArguments;
            Errors = impl.Errors;
        }


        class AlertNoteDefinitionImpl :
            AlertNoteDefinition
        {
            string _note;
            string _source;
            string _user;

            public AddAlertNoteRequest Request =>
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


        class AddAlertNoteQueryImpl :
            AddAlertNoteQuery
        {
            public Guid QueryIdentifier { get; private set; }
            public IDictionary<string, object> QueryArguments { get; }
            public List<Error> Errors = new();

            public AddAlertNoteQueryImpl()
            {
                QueryArguments = new Dictionary<string, object>();
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