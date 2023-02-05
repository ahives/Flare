using Flare.API.Model;
using Flare.Extensions;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> Close(Guid identifier, Action<AlertClosure> action, Action<AlertClosureQuery> query, CancellationToken cancellationToken = default)
    {
        var impl = new AlertClosureImpl();
        action?.Invoke(impl);

        var request = impl.Request;

        return await Close(identifier, request, query, cancellationToken);
    }

    public async Task<Result> Close(Guid identifier, CloseAlertRequest request, Action<AlertClosureQuery> query, CancellationToken cancellationToken = default)
    {
        var impl = new AlertClosureQueryImpl();
        query?.Invoke(impl);

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}/close"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}/close?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url, Request = request.ToJsonString()}};
    }


    class AlertClosureImpl :
        AlertClosure
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

    class AlertClosureQueryImpl :
        AlertClosureQuery
    {
        public IDictionary<string, object> QueryArguments { get; }

        public AlertClosureQueryImpl()
        {
            QueryArguments = new Dictionary<string, object>();
        }

        public void SearchIdentifierType(CloseSearchIdentifierType type)
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