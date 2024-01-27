namespace Flare.Alert.Internal;

using System.Collections.Immutable;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<ResultInfo>> Create(Action<CreateAlertCriteria> criteria, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new CreateAlertCriteriaImpl();
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Count != 0)
            return Response.Failed<ResultInfo>(Debug.WithErrors("alerts", errors));

        return await PostRequest<ResultInfo, CreateAlertRequest>("alerts", impl.Request, Serializer.Options, cancellationToken).ConfigureAwait(false);
    }


    class CreateAlertCriteriaImpl :
        CreateAlertCriteria,
        IQueryCriteria,
        IValidator
    {
        string _description;
        string _alias;
        string _notes;
        List<string> _actions;
        List<AlertTag> _tags;
        string _entity;
        AlertPriority _priority;
        List<Responder> _responders;
        List<VisibleResponder> _visibility;
        IDictionary<string, string> _details;
        string _message;
        string _source;
        string _user;

        public CreateAlertRequest Request =>
            new()
            {
                Description = _description,
                Responders = _responders,
                VisibleTo = _visibility,
                Alias = _alias,
                Note = _notes,
                Details = _details,
                Actions = _actions,
                Tags = _tags,
                Entity = _entity,
                Source = _source,
                User = _user,
                Priority = _priority
            };

        public CreateAlertCriteriaImpl()
        {
            _priority = AlertPriority.P3;
            _tags = new List<AlertTag>();
            _responders = new List<Responder>();
            _visibility = new List<VisibleResponder>();
            _details = new Dictionary<string, string>();
            _actions = new List<string>();
        }

        public void Description(string description)
        {
            _description = description;
        }

        public void Responders(Action<AddResponder> action)
        {
            var impl = new AddResponderImpl();
            action?.Invoke(impl);

            _responders = impl.Recipients;
        }

        public void VisibleTo(Action<VisibleTo> action)
        {
            var impl = new VisibleToImpl();
            action?.Invoke(impl);

            _visibility = impl.Visibility;
        }

        public void Alias(string alias)
        {
            _alias = alias;
        }

        public void Source(string source)
        {
            _source = source;
        }

        public void User(string user)
        {
            _user = user;
        }

        public void AdditionalNotes(string notes)
        {
            _notes = notes;
        }

        public void CustomProperties(Action<AlertProperty> action)
        {
            var impl = new AlertPropertyImpl();
            action?.Invoke(impl);

            _details = impl.Properties;
        }

        public void CustomActions(string action, params string[] actions)
        {
            var temp = new List<string> {action};

            for (int i = 0; i < actions.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(actions[i]))
                    continue;
                temp.Add(actions[i]);
            }

            _actions = temp;
        }

        public void Tags(Action<TagBuilder> action)
        {
            var impl = new TagBuilderImpl();
            action?.Invoke(impl);

            _tags = impl.Tags;
        }

        public void RelatedToDomain(string entity)
        {
            _entity = entity;
        }

        public void Priority(AlertPriority priority)
        {
            _priority = priority;
        }

        public void Message(string message)
        {
            _message = message;
        }

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();
            if (string.IsNullOrWhiteSpace(_message))
                errors.Add(Errors.Create(ErrorType.Message, "Message is required to create an alert."));

            if (!string.IsNullOrWhiteSpace(_message) && _message.Length > 130)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The message property has a limit of 100 character."));

            if (!string.IsNullOrWhiteSpace(_alias) && _alias.Length > 512)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The alias property has a limit of 100 character."));

            if (!string.IsNullOrWhiteSpace(_description) && _description.Length > 15000)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "The description property has a limit of 100 character."));

            if (_responders.Count > 50)
                errors.Add(Errors.Create(ErrorType.RespondersLimitExceeded, "You can only have 50 responders."));

            if (_visibility.Count > 50)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "You can only have 50 teams or users this alert can be visible to."));

            if (_actions.Count > 50)
                errors.Add(Errors.Create(ErrorType.ActionsLimitExceeded, "You can have no more than 50 actions on a single alert."));

            int totalActionLength = TotalLength(_actions);
            if (totalActionLength > 500)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, $"The total length of actions is {totalActionLength}. You can have no more than 50 actions on a single alert totaling 500 characters."));

            if (_tags.Count > 50)
                errors.Add(Errors.Create(ErrorType.TagsLimitExceeded, "You can have no more than 50 tags on a single alert."));

            // int totalTagsLength = TotalLength(_tags);
            // if (totalTagsLength > 1000)
            //     errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, $"The total length of actions is {totalTagsLength}. You can have no more than 50 tags on a single alert totaling 1000 characters."));

            if (TotalLength(_details.Values.ToList()) > 8000)
                errors.Add(Errors.Create(ErrorType.CustomPropertiesLimitExceeded, "You can have no more than 50 tags on a single alert."));

            if (!string.IsNullOrWhiteSpace(_entity) && _entity.Length > 512)
                errors.Add(Errors.Create(ErrorType.TagsLimitExceeded, "You can have no more than 50 tags on a single alert."));

            if (!string.IsNullOrWhiteSpace(_source) && _source.Length > 100)
                errors.Add(Errors.Create(ErrorType.SourceCharLimitExceeded, "You can have no more than 100 characters to represent the source of the alert."));

            if (!string.IsNullOrWhiteSpace(_user) && _user.Length > 100)
                errors.Add(Errors.Create(ErrorType.UserCharLimitExceeded, "You can have no more than 100 characters to represent the display name of the request owner."));

            if (!string.IsNullOrWhiteSpace(_notes) && _notes.Length > 100)
                errors.Add(Errors.Create(ErrorType.StringLengthLimitExceeded, "You can have no more than 25000 characters to the note."));

            return errors;

            int TotalLength(List<string> items)
            {
                int length = 0;
                for (int i = 0; i < items.Count; i++)
                    length += items[i].Length;

                return length;
            }
        }

        public Dictionary<string, QueryArg> GetQueryArguments() => new(ImmutableDictionary<string, QueryArg>.Empty);


        class TagBuilderImpl :
            TagBuilder
        {
            public List<AlertTag> Tags { get; } = new();

            public void Add(AlertTag tag)
            {
                Tags.Add(tag);
            }
        }


        class AlertPropertyImpl :
            AlertProperty
        {
            public IDictionary<string, string> Properties { get; }

            public AlertPropertyImpl()
            {
                Properties = new Dictionary<string, string>();
            }

            public void Add(string name, string value)
            {
                Properties.Add(name, value);
            }
        }


        class AddResponderImpl :
            AddResponder
        {
            public List<Responder> Recipients { get; }

            public AddResponderImpl()
            {
                Recipients = new List<Responder>();
            }

            public void Team(Action<TeamResponder> action)
            {
                var impl = new TeamResponderImpl();
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            public void User(Action<UserResponder> action)
            {
                var impl = new UserResponderImpl();
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            public void Escalation(Action<EscalationResponder> action)
            {
                var impl = new EscalationResponderImpl();
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            public void Schedule(Action<ScheduleResponder> action)
            {
                var impl = new ScheduleResponderImpl();
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }


            class EscalationResponderImpl :
                EscalationResponder
            {
                public Responder Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = new Responder {Id = id, Type = ResponderType.Escalation};
                }

                public void Name(string name)
                {
                    Data = new Responder {Name = name, Type = ResponderType.Escalation};
                }
            }


            class ScheduleResponderImpl :
                ScheduleResponder
            {
                public Responder Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = new Responder {Id = id, Type = ResponderType.Schedule};
                }

                public void Name(string name)
                {
                    Data = new Responder {Name = name, Type = ResponderType.Schedule};
                }
            }


            class UserResponderImpl :
                UserResponder
            {
                public Responder Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = new Responder {Id = id, Type = ResponderType.User};
                }

                public void Username(string username)
                {
                    Data = new Responder {Username = username, Type = ResponderType.User};
                }
            }


            class TeamResponderImpl :
                TeamResponder
            {
                public Responder Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = new Responder {Id = id, Type = ResponderType.Team};
                }

                public void Name(string name)
                {
                    Data = new Responder {Name = name, Type = ResponderType.Team};
                }
            }
        }


        class VisibleToImpl :
            VisibleTo
        {
            public List<VisibleResponder> Visibility { get; }

            public VisibleToImpl()
            {
                Visibility = new List<VisibleResponder>();
            }

            public void Team(Action<VisibleToTeam> action)
            {
                var impl = new VisibleToTeamImpl();
                action?.Invoke(impl);

                Visibility.Add(impl.VisibleResponder);
            }

            public void User(Action<VisibleToUser> action)
            {
                var impl = new VisibleToUserImpl();
                action?.Invoke(impl);

                Visibility.Add(impl.VisibleResponder);
            }

            
            class VisibleToTeamImpl :
                VisibleToTeam
            {
                public VisibleResponder VisibleResponder { get; private set; }
                
                public void Id(Guid id)
                {
                    VisibleResponder = new VisibleResponder {Id = id, Type = ResponderType.Team};
                }

                public void Name(string name)
                {
                    VisibleResponder = new VisibleResponder {Name = name, Type = ResponderType.Team};
                }
            }


            class VisibleToUserImpl :
                VisibleToUser
            {
                public VisibleResponder VisibleResponder { get; private set; }
                
                public void Id(Guid id)
                {
                    VisibleResponder = new VisibleResponder {Id = id, Type = ResponderType.User};
                }

                public void Username(string username)
                {
                    VisibleResponder = new VisibleResponder {Username = username, Type = ResponderType.User};
                }
            }
        }
    }
}