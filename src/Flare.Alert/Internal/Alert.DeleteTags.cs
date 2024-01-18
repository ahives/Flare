namespace Flare.Alert.Internal;

using System.Text;
using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertTagInfo>> DeleteTags(string identifier, IdentifierType identifierType, Action<DeleteAlertTagsCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertTagsCriteriaImpl();
        criteria?.Invoke(impl);

        var qc = impl as IQueryCriteria;

        var errors = new List<Error>();
        errors.AddRange(Validate());
        errors.AddRange(qc.Validate());

        if (errors.Count != 0)
            return Response.Failed<AlertTagInfo>(
                Debug.WithErrors("alerts/{identifier}/tags?identifierType={idType}", errors));

        var args = qc.GetQueryArguments();
        string url = args.Count > 0
            ? $"alerts/{identifier}/tags?identifierType={GetIdentifierType()}&{args.BuildQueryString()}"
            : $"alerts/{identifier}/tags?identifierType={GetIdentifierType()}";

        return await DeleteRequest<AlertTagInfo>(url, Serializer.Options, cancellationToken);

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


    class DeleteAlertTagsCriteriaImpl :
        DeleteAlertTagsCriteria,
        IQueryCriteria
    {
        string _notes;
        string _source;
        string _user;
        List<AlertTag> _tags;

        public void Tags(Action<TagBuilder> action)
        {
            var impl = new TagBuilderImpl();
            action?.Invoke(impl);

            _tags = impl.Tags;
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

            if (!_tags.Any())
                errors.Add(Errors.Create(ErrorType.AlertTagsMissing, "The tags are missing."));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            StringBuilder builder = new StringBuilder();
        
            for (int i = 0; i < _tags.Count; i++)
            {
                if (i == 0)
                {
                    builder.AppendFormat(_tags[i].ToString());
                    continue;
                }
            
                builder.AppendFormat($",{_tags[i].ToString()}");
            }

            return new Dictionary<string, QueryArg>
            {
                {"tags", new QueryArg {Value = builder.ToString()}},
                {"user", new QueryArg {Value = _user}},
                {"source", new QueryArg {Value = _source}},
                {"note", new QueryArg {Value = _notes}}
            };
        }


        class TagBuilderImpl :
            TagBuilder
        {
            public List<AlertTag> Tags { get; } = new();

            public void Add(AlertTag tag)
            {
                Tags.Add(tag);
            }
        }
    }
}