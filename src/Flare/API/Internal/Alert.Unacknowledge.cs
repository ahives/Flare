namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<UnacknowledgeAlertInfo>> Unacknowledge(Guid identifier, IdentifierType identifierType, Action<UnacknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new UnacknowledgeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string url =
            $"https://api.opsgenie.com/v2/alerts/{identifier}/unacknowledge?identifierType={GetIdentifierType(identifierType)}";

        return await PostRequest<UnacknowledgeAlertInfo, UnacknowledgeAlertRequest>(url, impl.Request, cancellationToken);

        string GetIdentifierType(IdentifierType type) =>
            type switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };
    }


    class UnacknowledgeAlertCriteriaImpl :
        UnacknowledgeAlertCriteria
    {
        string _note;
        string _source;
        string _user;

        public UnacknowledgeAlertRequest Request =>
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
}