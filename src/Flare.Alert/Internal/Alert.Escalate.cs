namespace Flare.Alert.Internal;

using System.Collections.Immutable;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<EscalateAlertInfo>> Escalate(string identifier, IdentifierType identifierType,
        Action<EscalateAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new EscalateAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = new List<Error>();
        errors.AddRange(Validate());
        errors.AddRange(qc.Validate());

        if (errors.Count != 0)
            return Response.Failed<EscalateAlertInfo>(
                Debug.WithErrors("alerts/{identifier}/escalate?identifierType={idType}", errors));

        string url =
            $"alerts/{identifier}/escalate?identifierType={GetIdentifierType()}";

        return await PostRequest<EscalateAlertInfo, EscalateAlertRequest>(url, impl.Request, Serializer.Options,
            cancellationToken);

        string GetIdentifierType() =>
            identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };

        IReadOnlyList<Error> Validate()
        {
            bool isIdentifierTypeMissing = identifierType switch
            {
                IdentifierType.Id => false,
                IdentifierType.Tiny => false,
                IdentifierType.Alias => false,
                _ => true
            };

            var errors = new List<Error>();

            if (isIdentifierTypeMissing)
                errors.Add(Errors.Create(ErrorType.IdentifierType,
                    $"{identifierType.ToString()} is not a valid identifier type in the current context."));

            bool isGuid = Guid.TryParse(identifier, out _);

            if (isGuid && identifierType != IdentifierType.Id)
                errors.Add(Errors.Create(ErrorType.IdentifierType,
                    "Identifier type is not compatible with identifier."));

            if (isGuid && identifier != default && isIdentifierTypeMissing ||
                (string.IsNullOrWhiteSpace(identifier) && isIdentifierTypeMissing))
                errors.Add(Errors.Create(ErrorType.IdentifierType, "Identifier type is missing."));

            return errors;
        }
    }


    class EscalateAlertCriteriaImpl :
        EscalateAlertCriteria,
        IQueryCriteria
    {
        string _note;
        string _source;
        string _user;
        Escalation? _escalation;

        public EscalateAlertRequest Request =>
            new()
            {
                Escalation = _escalation,
                Note = _note,
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

        public void Note(string note)
        {
            _note = note;
        }

        public void Escalation(Action<EscalationAlertIdentifier> action)
        {
            var impl = new EscalationAlertIdentifierImpl();
            action?.Invoke(impl);

            _escalation = impl.Escalation;
        }

        public bool IsSearchQuery() => false;

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();
            if (_user.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The user property has a limit of 100 character."));

            if (_source.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The source property has a limit of 100 character."));

            if (_note.Length > 25000)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The note property has a limit of 25,000 character."));

            if (_escalation is null)
                errors.Add(Errors.Create(ErrorType.EscalationMissing, "The escalation property is missing."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);


        class EscalationAlertIdentifierImpl :
            EscalationAlertIdentifier
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