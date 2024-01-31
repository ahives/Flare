namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertRecipientInfo>> GetAllRecipients(string identifier, IdentifierType identifierType, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new GetAllAlertsRecipientsImpl(identifier, identifierType);

        var errors = impl.Validate();
        if (errors.Any())
            return Response.Failed<AlertRecipientInfo>(Debug.WithErrors("alerts/{identifier}/recipients?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/recipients{impl.GetQueryArguments().BuildQueryString()}";

        return await GetRequest<AlertRecipientInfo>(url, Serializer.Options, cancellationToken);
    }


    class GetAllAlertsRecipientsImpl :
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;

        public GetAllAlertsRecipientsImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

        public IReadOnlyList<Error> Validate()
        {
            return _identifier.ValidateIdType(_identifierType, t => t switch
            {
                IdentifierType.Id => false,
                IdentifierType.Tiny => false,
                IdentifierType.Alias => false,
                _ => true
            });
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            string identifierType = _identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };

            return string.IsNullOrWhiteSpace(identifierType)
                ? new Dictionary<string, QueryArg>()
                : new Dictionary<string, QueryArg> {{"identifierType", new QueryArg {Value = identifierType}}};
        }
    }
}