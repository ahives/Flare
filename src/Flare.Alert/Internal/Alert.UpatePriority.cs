namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> UpdatePriority(string identifier, IdentifierType identifierType, AlertPriority priority,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new UpdateAlertPriorityImpl(identifier, identifierType);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts{identifier}/priority?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/priority{impl.GetQueryArguments().BuildQueryString()}";

        return await PutRequest<ResultInfo, UpdateAlertPriorityRequest>(url,
            new UpdateAlertPriorityRequest {Priority = priority}, Serializer.Options, cancellationToken);
    }


    class UpdateAlertPriorityImpl :
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;

        public UpdateAlertPriorityImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();

            errors.AddRange(_identifier.ValidateIdType(_identifierType, t => t switch
            {
                IdentifierType.Id => false,
                IdentifierType.Tiny => false,
                IdentifierType.Alias => false,
                _ => true
            }));

            return errors;
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