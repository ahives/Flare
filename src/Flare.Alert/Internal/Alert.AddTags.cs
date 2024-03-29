namespace Flare.Alert.Internal;

using System.Text;
using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> AddTags(string identifier, IdentifierType identifierType, Action<AddAlertTagsCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new AddAlertTagsImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts/{identifier}/tags?identifierType={idType}", errors));

        string url = $"alerts/{identifier}/tags{impl.GetQueryArguments().BuildQueryString()}";

        return await PostRequest<ResultInfo, AddAlertTagsRequest>(url, impl.Request, Serializer.Options, cancellationToken);
    }


    class AddAlertTagsImpl :
        AddAlertTagsCriteria,
        IQueryCriteria,
        IValidator
    {
        string _identifier;
        IdentifierType _identifierType;
        string _notes;
        string _source;
        string _user;
        string _tags;

        public AddAlertTagsRequest Request =>
            new()
            {
                Tags = _tags,
                Notes = _notes,
                Source = _source,
                User = _user
            };

        public AddAlertTagsImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

        public void Tags(Action<TagBuilder> action)
        {
            var impl = new TagBuilderImpl();
            action?.Invoke(impl);

            StringBuilder builder = new StringBuilder();
        
            for (int i = 0; i < impl.Tags.Count; i++)
            {
                if (i == 0)
                {
                    builder.AppendFormat(impl.Tags[i].ToString());
                    continue;
                }
            
                builder.AppendFormat($",{impl.Tags[i].ToString()}");
            }

            _tags = builder.ToString();
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

            if (!string.IsNullOrWhiteSpace(_tags) && _tags.Length > 1000)
                errors.Add(Errors.Create(ErrorType.AlertTagsMissing, "The alert responder is missing."));

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