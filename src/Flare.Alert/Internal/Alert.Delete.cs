namespace Flare.Alert.Internal;

using System.Collections.ObjectModel;
using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<DeleteAlertInfo>> Delete(string identifier, IdentifierType identifierType, Action<DeleteAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = new List<Error>();
        errors.AddRange(Validate());
        errors.AddRange(qc.Validate());

        var arguments = qc.GetQueryArguments();
        string url = arguments.Count > 0
            ? $"alerts/{identifier}?identifierType={GetIdentifierType()}&{QueryExtensions.BuildQueryString(arguments)}"
            : $"alerts/{identifier}?identifierType={GetIdentifierType()}";

        return await DeleteRequest<DeleteAlertInfo>(url, Serializer.Options, cancellationToken).ConfigureAwait(false);

        string GetIdentifierType() =>
            identifierType switch
            {
                IdentifierType.AlertId => "AlertID",
                IdentifierType.TinyId => "tinyID",
                _ => string.Empty
            };

        IReadOnlyList<Error> Validate()
        {
            bool isIdentifierTypeMissing = identifierType switch
            {
                IdentifierType.AlertId => false,
                IdentifierType.TinyId => false,
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

    
    class DeleteAlertCriteriaImpl :
        DeleteAlertCriteria,
        IQueryCriteria
    {
        private string _source;
        private string _user;

        public void User(string displayName)
        {
            _user = displayName;
        }

        public void Source(string displayName)
        {
            _source = displayName;
        }

        public bool IsSearchQuery() => false;

        public IReadOnlyList<Error> Validate() => ReadOnlyCollection<Error>.Empty;

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            var arguments = new Dictionary<string, QueryArg>();

            if (!string.IsNullOrWhiteSpace(_user))
                arguments.Add("user", new QueryArg {Value = _user});

            if (!string.IsNullOrWhiteSpace(_source))
                arguments.Add("source", new QueryArg {Value = _source});

            return arguments;
        }
    }
}