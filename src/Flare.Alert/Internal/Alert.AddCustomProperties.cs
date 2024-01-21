namespace Flare.Alert.Internal;

using System.Collections.Immutable;
using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> AddCustomProperties(string identifier, IdentifierType identifierType, Action<AddAlertCustomPropertiesCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AddAlertCustomPropertiesCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = Validate().Concat(qc.Validate()).ToList();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(
                Debug.WithErrors("alerts/{identifier}/details?identifierType={idType}", errors));

        string url =
            $"alerts/{identifier}/details?identifierType={GetIdentifierType()}";

        return await PostRequest<ResultInfo, AddAlertCustomPropertiesRequest>(url, impl.Request, Serializer.Options,
            cancellationToken);

        string GetIdentifierType() =>
            identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };

        IReadOnlyList<Error> Validate() =>
            identifier.ValidateIdType(identifierType, t => t switch
            {
                IdentifierType.Id => false,
                IdentifierType.Tiny => false,
                IdentifierType.Alias => false,
                _ => true
            });
    }


    class AddAlertCustomPropertiesCriteriaImpl :
        AddAlertCustomPropertiesCriteria,
        IQueryCriteria
    {
        string _notes;
        string _source;
        string _user;
        Dictionary<string, string> _details;

        public AddAlertCustomPropertiesCriteriaImpl()
        {
            _details = new Dictionary<string, string>();
        }

        public AddAlertCustomPropertiesRequest Request =>
            new()
            {
                Details = _details,
                Notes = _notes,
                Source = _source,
                User = _user
            };

        public void Details(Action<CustomPropertyBuilder> action)
        {
            var impl = new CustomPropertyBuilderImpl();
            action?.Invoke(impl);

            _details = impl.Details;
        }

        class CustomPropertyBuilderImpl : CustomPropertyBuilder
        {
            public Dictionary<string, string> Details { get; } = new();

            public void Add(string key, string value)
            {
                Details.Add(key, value);
            }
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

        public bool IsSearchQuery() => false;

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

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);
    }
}