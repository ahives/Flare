namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> DeleteCustomProperties(string identifier, IdentifierType identifierType, Action<DeleteAlertCustomPropertiesCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertCustomPropertiesCriteriaImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}/details", errors));

        string url = $"alerts/{identifier}/details{impl.GetQueryArguments().BuildQueryString()}";

        return await DeleteRequest<ResultInfo>(url, Serializer.Options, cancellationToken);
    }


    class DeleteAlertCustomPropertiesCriteriaImpl :
        DeleteAlertCustomPropertiesCriteria,
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;
        string _notes;
        string _source;
        string _user;
        List<string> _details;

        public DeleteAlertCustomPropertiesCriteriaImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
            _details = new List<string>();
        }

        class CustomPropertyKeyBuilderImpl : CustomPropertyKeyBuilder
        {
            public List<string> Details { get; } = new();

            public void Add(string key)
            {
                Details.Add(key);
            }
        }

        public void Details(Action<CustomPropertyKeyBuilder> action)
        {
            var impl = new CustomPropertyKeyBuilderImpl();
            action?.Invoke(impl);

            _details = impl.Details;
        }

        public void User(string displayName)
        {
            _user = displayName;
        }

        public void Source(string displayName)
        {
            _source = displayName;
        }

        public void Notes(string notes)
        {
            _notes = notes;
        }

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();
            if (!string.IsNullOrWhiteSpace(_user) && _user.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The user property has a limit of 100 character."));

            if (!string.IsNullOrWhiteSpace(_source) && _source.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The source property has a limit of 100 character."));

            if (!string.IsNullOrWhiteSpace(_notes) && _notes.Length > 25000)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The note property has a limit of 25,000 character."));

            // if (!string.IsNullOrWhiteSpace(_details) && _details.Length > 1000)
            //     errors.Add(Errors.Create(ErrorType.AlertTagsMissing, "The alert responder is missing."));

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

            var arguments = new Dictionary<string, QueryArg>();

            if (!string.IsNullOrWhiteSpace(identifierType))
                arguments.Add("identifierType", new QueryArg {Value = identifierType});

            string keys = string.Join(',', _details);
            if (!string.IsNullOrWhiteSpace(keys))
                arguments.Add("keys", new QueryArg {Value = keys});

            if (!string.IsNullOrWhiteSpace(_user))
                arguments.Add("user", new QueryArg {Value = _user});

            if (!string.IsNullOrWhiteSpace(_source))
                arguments.Add("source", new QueryArg {Value = _source});

            if (!string.IsNullOrWhiteSpace(_notes))
                arguments.Add("note", new QueryArg {Value = _notes});

            return arguments;
        }
    }
}