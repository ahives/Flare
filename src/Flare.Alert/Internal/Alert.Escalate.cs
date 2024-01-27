namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> Escalate(string identifier, IdentifierType identifierType,
        Action<EscalateAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new EscalateAlertCriteriaImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}/escalate?identifierType={idType}", errors));

        string url = $"alerts/{identifier}/escalate{impl.GetQueryArguments().BuildQueryString()}";

        return await PostRequest<ResultInfo, EscalateAlertRequest>(url, impl.Request, Serializer.Options, cancellationToken);
    }


    class EscalateAlertCriteriaImpl :
        EscalateAlertCriteria,
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;
        string _notes;
        string _source;
        string _user;
        Escalation? _escalation;

        public EscalateAlertRequest Request =>
            new()
            {
                Escalation = _escalation,
                Notes = _notes,
                Source = _source,
                User = _user
            };

        public EscalateAlertCriteriaImpl(string identifier, IdentifierType identifierType)
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

        public void Notes(string notes)
        {
            _notes = notes;
        }

        public void Escalation(Action<AlertEscalationIdentifier> action)
        {
            var impl = new AlertEscalationIdentifierImpl();
            action?.Invoke(impl);

            _escalation = impl.Escalation;
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

            if (_escalation is null)
                errors.Add(Errors.Create(ErrorType.AlertEscalationMissing, "The escalation property is missing."));

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


        class AlertEscalationIdentifierImpl :
            AlertEscalationIdentifier
        {
            string _name;
            Guid _id;

            public Escalation Escalation => new()
            {
                Id = _id,
                Name = _name
            };

            public void Id(Guid identifier)
            {
                _id = identifier;
            }

            public void Name(string name)
            {
                _name = name;
            }
        }
    }
}