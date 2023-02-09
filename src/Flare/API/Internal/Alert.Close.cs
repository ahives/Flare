using Flare.API.Model;
using Flare.Extensions;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> Close(Action<CloseAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new CloseAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/close"
            : $"https://api.opsgenie.com/v2/alerts/{impl.Identifier}/close?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url, Request = impl.Request.ToJsonString()}};
    }


    class CloseAlertCriteriaImpl :
        CloseAlertCriteria
    {
        public Guid Identifier { get; private set; }
        public IDictionary<string, object> QueryArguments { get; private set; }
        public CloseAlertRequest Request { get; private set; }

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

            public CloseAlertQueryImpl()
            {
                QueryArguments = new Dictionary<string, object>();
            }

            public void Identifier(Guid identifier)
            {
                QueryIdentifier = identifier;
            }

            public void IdentifierType(CloseSearchIdentifierType type)
            {
                string searchIdentifierType = type switch
                {
                    CloseSearchIdentifierType.Id => "id",
                    CloseSearchIdentifierType.Tiny => "tiny",
                    CloseSearchIdentifierType.Alias => "alias",
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
            
                QueryArguments.Add("identifierType", searchIdentifierType);
            }
        }
    }
}