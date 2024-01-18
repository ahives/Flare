namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> Delete(string identifier, IdentifierType identifierType, Action<DeleteAlertCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = Validate().Concat(qc.Validate()).ToList();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}?identifierType={identifierType}", errors));

        var arguments = qc.GetQueryArguments();
        string url = arguments.Count > 0
            ? $"alerts/{identifier}?identifierType={GetIdentifierType()}&{arguments.BuildQueryString()}"
            : $"alerts/{identifier}?identifierType={GetIdentifierType()}";

        return await DeleteRequest<ResultInfo>(url, Serializer.Options, cancellationToken).ConfigureAwait(false);

        string GetIdentifierType() =>
            identifierType switch
            {
                IdentifierType.AlertId => "AlertID",
                IdentifierType.TinyId => "tinyID",
                _ => string.Empty
            };

        IReadOnlyList<Error> Validate() =>
            identifier.ValidateIdType(identifierType, t => t switch
            {
                IdentifierType.AlertId => false,
                IdentifierType.TinyId => false,
                _ => true
            });
    }

    
    class DeleteAlertCriteriaImpl :
        DeleteAlertCriteria,
        IQueryCriteria
    {
        string _source;
        string _user;

        public void User(string displayName)
        {
            _user = displayName;
        }

        public void Source(string displayName)
        {
            _source = displayName;
        }

        public bool IsSearchQuery() => false;

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();
            if (!string.IsNullOrWhiteSpace(_user) && _user.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The user property has a limit of 100 character."));

            if (!string.IsNullOrWhiteSpace(_source) && _source.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The source property has a limit of 100 character."));

            return errors;
        }

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