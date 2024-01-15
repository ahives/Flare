namespace Flare.Alert.Internal;

using System.Collections.Immutable;
using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertResponderInfo>> AddResponder(string identifier, IdentifierType identifierType, Action<AddAlertResponderCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AddAlertResponderCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = new List<Error>();
        errors.AddRange(Validate());
        errors.AddRange(qc.Validate());

        if (errors.Count != 0)
            return Response.Failed<AlertResponderInfo>(
                Debug.WithErrors("alerts/{identifier}/responders?identifierType={idType}", errors));

        string url =
            $"alerts/{identifier}/responders?identifierType={GetIdentifierType()}";

        return await PostRequest<AlertResponderInfo, AddAlertResponderRequest>(url, impl.Request, Serializer.Options,
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


    class AddAlertResponderCriteriaImpl :
        AddAlertResponderCriteria,
        IQueryCriteria
    {
        string _notes;
        string _source;
        string _user;
        Recipient? _responder;

        public AddAlertResponderRequest Request =>
            new()
            {
                Responder = _responder,
                Notes = _notes,
                Source = _source,
                User = _user
            };

        public void Responder(Action<AlertResponderIdentifier> action)
        {
            var impl = new AlertResponderIdentifierImpl();
            action?.Invoke(impl);

            _responder = impl.Responder;
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

            if (_responder is null)
                errors.Add(Errors.Create(ErrorType.AlertResponderMissing, "The alert responder is missing."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);


        class AlertResponderIdentifierImpl :
            AlertResponderIdentifier
        {
            Guid _id;
            string _username;
            RecipientType _responderType;

            public Recipient Responder => new()
            {
                Id = _id,
                Username = _username,
                Type = _responderType
            };

            public void Id(Guid id)
            {
                _id = id;
            }

            public void Type(RecipientType responder)
            {
                _responderType = responder;
            }

            public void Username(string username)
            {
                _username = username;
            }
        }
    }
}