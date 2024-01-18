namespace Flare.Alert.Internal;

using System.Collections.Immutable;
using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> AddNote(string identifier, IdentifierType identifierType,
        Action<AddAlertNoteCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AddAlertNoteCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = Validate().Concat(qc.Validate()).ToList();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}/notes?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/notes?identifierType={GetIdentifierType()}";

        return await PostRequest<ResultInfo, AddAlertNoteRequest>(url, impl.Request, Serializer.Options, cancellationToken);

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


    class AddAlertNoteCriteriaImpl :
        AddAlertNoteCriteria,
        IQueryCriteria
    {
        string _notes;
        string _source;
        string _user;

        public AddAlertNoteRequest Request =>
            new()
            {
                Notes = _notes,
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

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);
    }
}