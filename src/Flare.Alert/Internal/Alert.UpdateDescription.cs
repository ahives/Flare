namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> UpdateDescription(string identifier, IdentifierType identifierType, string description,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new UpdateAlertDescriptionImpl(identifier, identifierType);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts{identifier}/description?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/description{impl.GetQueryArguments().BuildQueryString()}";

        return await PutRequest<ResultInfo, UpdateAlertDescriptionRequest>(url,
            new UpdateAlertDescriptionRequest {Description = description}, Serializer.Options, cancellationToken);
    }


    class UpdateAlertDescriptionImpl :
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;

        public UpdateAlertDescriptionImpl(string identifier, IdentifierType identifierType)
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