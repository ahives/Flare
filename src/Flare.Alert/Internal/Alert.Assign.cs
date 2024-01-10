namespace Flare.Alert.Internal;

using System.Collections.Immutable;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AssignAlertInfo>> Assign(string identifier, IdentifierType identifierType,
        Action<AssignAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AssignAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = new List<Error>();
        errors.AddRange(Validate());
        errors.AddRange(qc.Validate());

        if (errors.Count != 0)
            return Response.Failed<AssignAlertInfo>(
                Debug.WithErrors("alerts/{identifier}/assign?identifierType={idType}", errors));

        string url =
            $"alerts/{identifier}/assign?identifierType={GetIdentifierType()}";

        return await PostRequest<AssignAlertInfo, AssignAlertRequest>(url, impl.Request, Serializer.Options,
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


    class AssignAlertCriteriaImpl :
        AssignAlertCriteria,
        IQueryCriteria
    {
        string _note;
        string _source;
        string _user;
        Owner? _owner;

        public AssignAlertRequest Request =>
            new()
            {
                Owner = _owner,
                Note = _note,
                Source = _source,
                User = _user
            };

        public void Owner(Action<AssignAlertIdentifier> action)
        {
            var impl = new AssignAlertIdentifierImpl();
            action?.Invoke(impl);

            _owner = impl.Owner;
        }

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

            if (_owner is null)
                errors.Add(Errors.Create(ErrorType.OwnerMissing, "The owner property is missing."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);


        class AssignAlertIdentifierImpl :
            AssignAlertIdentifier
        {
            string _username;
            Guid _id;

            public Owner Owner => new()
            {
                Id = _id,
                Username = _username
            };

            public void Id(Guid identifier)
            {
                _id = identifier;
            }

            public void Username(string name)
            {
                _username = name;
            }
        }
    }
}