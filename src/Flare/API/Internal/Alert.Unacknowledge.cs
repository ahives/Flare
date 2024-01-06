namespace Flare.API.Internal;

using Model;

public partial class AlertImpl
{
    public async Task<Maybe<UnacknowledgeAlertInfo>> Unacknowledge(string identifier, IdentifierType identifierType, Action<UnacknowledgeAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new UnacknowledgeAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var errors = Validate();
        if (errors.Count != 0)
            return Response.Failed<UnacknowledgeAlertInfo>(Debug.WithErrors("alerts/{identifier}/unacknowledge?identifierType={idType}", errors));

        string url =
            $"alerts/{identifier}/unacknowledge?identifierType={GetIdentifierType()}";

        return await PostRequest<UnacknowledgeAlertInfo, UnacknowledgeAlertRequest>(url, impl.Request, cancellationToken);

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


    class UnacknowledgeAlertCriteriaImpl :
        UnacknowledgeAlertCriteria
    {
        string _note;
        string _source;
        string _user;

        public UnacknowledgeAlertRequest Request =>
            new()
            {
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
    }
}