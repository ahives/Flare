namespace Flare.Alert.Internal;

using System.Text;
using Extensions;
using Flare.Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> DeleteTags(string identifier, IdentifierType identifierType, Action<DeleteAlertTagsCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new DeleteAlertTagsImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}/tags?identifierType={idType}", errors));

        string url = $"alerts/{identifier}/tags{impl.GetQueryArguments().BuildQueryString()}";

        return await DeleteRequest<ResultInfo>(url, Serializer.Options, cancellationToken);
    }


    class DeleteAlertTagsImpl :
        DeleteAlertTagsCriteria,
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;
        string _notes;
        string _source;
        string _user;
        List<AlertTag> _tags;

        public DeleteAlertTagsImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

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
            var arguments = new Dictionary<string, QueryArg>();

            string identifierType = _identifierType switch
            {
                IdentifierType.Id => "id",
                IdentifierType.Tiny => "tiny",
                IdentifierType.Alias => "alias",
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(identifierType))
                arguments.Add("identifierType", new QueryArg {Value = identifierType});

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

            string tags = builder.ToString();
            if (!string.IsNullOrWhiteSpace(tags))
                arguments.Add("tags", new QueryArg {Value = builder.ToString()});
            
            if (!string.IsNullOrWhiteSpace(_user))
                arguments.Add("user", new QueryArg {Value = _user});

            if (!string.IsNullOrWhiteSpace(_source))
                arguments.Add("source", new QueryArg {Value = _source});

            if (!string.IsNullOrWhiteSpace(_notes))
                arguments.Add("note", new QueryArg {Value = _notes});

            return arguments;
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