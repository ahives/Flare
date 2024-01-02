namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<AcknowledgeAlertInfo>> Acknowledge(Guid identifier, IdentifierType identifierType, Action<AcknowledgeAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AcknowledgeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        string url =
            $"https://api.opsgenie.com/v2/alerts/{identifier}/acknowledge?identifierType={GetIdentifierType(identifierType)}";

        return await PostRequest<AcknowledgeAlertInfo, AcknowledgeAlertRequest>(url, impl.Request, cancellationToken);

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
}