namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> Delete(string identifier, IdentifierType identifierType, Action<DeleteAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}{impl.GetQueryArguments().BuildQueryString()}";

        return await DeleteRequest<ResultInfo>(url, Serializer.Options, cancellationToken).ConfigureAwait(false);
    }

    
    class DeleteAlertImpl :
        DeleteAlertCriteria,
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;
        string _source;
        string _user;

        public DeleteAlertImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

        public void User(string displayName)
        {
            _user = displayName;
        }

        public void Source(string displayName)
        {
            _source = displayName;
        }

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();
            if (!string.IsNullOrWhiteSpace(_user) && _user.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The user property has a limit of 100 character."));

            if (!string.IsNullOrWhiteSpace(_source) && _source.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The source property has a limit of 100 character."));

            errors.AddRange(_identifier.ValidateIdType(_identifierType, t => t switch
            {
                IdentifierType.AlertId => false,
                IdentifierType.TinyId => false,
                _ => true
            }));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            string identifierType = _identifierType switch
            {
                IdentifierType.AlertId => "AlertID",
                IdentifierType.TinyId => "tinyID",
                _ => string.Empty
            };

            var arguments = new Dictionary<string, QueryArg>();

            if (!string.IsNullOrWhiteSpace(identifierType))
                arguments.Add("identifierType", new QueryArg {Value = identifierType});

            if (!string.IsNullOrWhiteSpace(_user))
                arguments.Add("user", new QueryArg {Value = _user});

            if (!string.IsNullOrWhiteSpace(_source))
                arguments.Add("source", new QueryArg {Value = _source});

            return arguments;
        }
    }
}