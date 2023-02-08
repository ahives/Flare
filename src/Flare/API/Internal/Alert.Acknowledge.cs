using Flare.API.Model;
using Flare.Extensions;

namespace Flare.API.Internal;

public partial class AlertImpl
{
    public async Task<Result> Acknowledge(Guid identifier, Action<AcknowledgeAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        var impl = new AcknowledgeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var request = impl.Request;

        string queryString = BuildQueryString(impl.QueryArguments);
        string url = string.IsNullOrWhiteSpace(queryString)
            ? $"https://api.opsgenie.com/v2/alerts/{identifier}"
            : $"https://api.opsgenie.com/v2/alerts/{identifier}?{queryString}";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo{URL = url, Request = request.ToJsonString()}};
    }


    class AcknowledgeAlertCriteriaImpl :
        AcknowledgeAlertCriteria
    {
        string _note;
        string _source;
        string _user;

        public IDictionary<string, object> QueryArguments { get; }

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