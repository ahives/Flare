using Flare.API.Model;
using Flare.Extensions;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> AddNote(Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new AddAlertNoteCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/notes"
            : $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/notes?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url, Request = impl.Request.ToJsonString()}};
    }


    class AddAlertNoteCriteriaImpl :
        AddAlertNoteCriteria
    {
        public Guid Identifier { get; private set; }
        public IDictionary<string, object> QueryArguments { get; private set; }
        public AddAlertNoteRequest Request { get; private set; }

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

            public AddAlertNoteQueryImpl()
            {
                QueryArguments = new Dictionary<string, object>();
            }

            public void Identifier(Guid identifier)
            {
                QueryIdentifier = identifier;
            }

            public void IdentifierType(AddAlertNoteIdentifierType type)
            {
                string searchIdentifierType = type switch
                {
                    AddAlertNoteIdentifierType.Id => "id",
                    AddAlertNoteIdentifierType.Tiny => "tiny",
                    AddAlertNoteIdentifierType.Alias => "alias",
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
            
                QueryArguments.Add("identifierType", searchIdentifierType);
            }
        }
    }
}