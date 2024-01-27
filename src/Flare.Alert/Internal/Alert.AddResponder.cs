namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> AddResponder(string identifier, IdentifierType identifierType, Action<AddAlertResponderCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AddAlertResponderImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}/responders?identifierType={idType}", errors));

        string url = $"alerts/{identifier}/responders{impl.GetQueryArguments().BuildQueryString()}";

        return await PostRequest<ResultInfo, AddAlertResponderRequest>(url, impl.Request, Serializer.Options, cancellationToken);
    }


    class AddAlertResponderImpl :
        AddAlertResponderCriteria,
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;
        string _notes;
        string _source;
        string _user;
        Responder? _responder;

        public AddAlertResponderRequest Request =>
            new()
            {
                Responder = _responder,
                Notes = _notes,
                Source = _source,
                User = _user
            };

        public AddAlertResponderImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

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


        class AlertResponderIdentifierImpl :
            AlertResponderIdentifier
        {
            Guid _id;
            string _username;
            ResponderType _responderType;

            public Responder Responder => new()
            {
                Id = _id,
                Username = _username,
                Type = _responderType
            };

            public void Id(Guid id)
            {
                _id = id;
            }

            public void Type(ResponderType responder)
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