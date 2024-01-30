namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> UpdateMessage(string identifier, IdentifierType identifierType, string message,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new UpdateAlertMessageImpl(identifier, identifierType);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts{identifier}/message?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/message{impl.GetQueryArguments().BuildQueryString()}";

        return await PutRequest<ResultInfo, UpdateAlertMessageRequest>(url,
            new UpdateAlertMessageRequest {Message = message}, Serializer.Options, cancellationToken);
    }


    class UpdateAlertMessageImpl :
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;

        public UpdateAlertMessageImpl(string identifier, IdentifierType identifierType)
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