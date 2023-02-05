using Flare.API.Model;
using Flare.Extensions;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> Acknowledge(Guid identifier, Action<AlertAcknowledge> action, Action<AlertAcknowledgeQuery> query, CancellationToken cancellationToken = default)
    {
        var impl = new AlertAcknowledgeImpl();
        action?.Invoke(impl);

        var request = impl.Request;

        return await Acknowledge(identifier, request, query, cancellationToken);
    }

    public async Task<Result> Acknowledge(Guid identifier, AcknowledgeAlertRequest request, Action<AlertAcknowledgeQuery> query,
        CancellationToken cancellationToken = default)
    {
        var impl = new AlertAcknowledgeQueryImpl();
        query?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url, Request = request.ToJsonString()}};
    }


    class AlertAcknowledgeImpl :
        AlertAcknowledge
    {
        string _note;
        string _source;
        string _user;

        public AcknowledgeAlertRequest Request =>
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

    class AlertAcknowledgeQueryImpl :
        AlertAcknowledgeQuery
    {
        public IDictionary<string, object> QueryArguments { get; }

        public AlertAcknowledgeQueryImpl()
        {
            QueryArguments = new Dictionary<string, object>();
        }

        public void SearchIdentifierType(AcknowledgeSearchIdentifierType type)
        {
            string searchIdentifierType = type switch
            {
                AcknowledgeSearchIdentifierType.Id => "id",
                AcknowledgeSearchIdentifierType.Tiny => "tiny",
                AcknowledgeSearchIdentifierType.Alias => "alias",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            QueryArguments.Add("identifierType", searchIdentifierType);
        }
    }
}