using Flare.API.Model;
using Flare.Configuration;
using Flare.Extensions;
using Flare.Model;

namespace Flare.API.Internal;

public partial class AlertImpl :
    FlareHttpClient,
    Alert
{
    public AlertImpl(FlareConfig config) : base(config)
    {
    }

    public async Task<Result> Create(Action<AlertDefinition> action, CancellationToken cancellationToken = default)
    {
        var impl = new AlertDefinitionImpl();
        action?.Invoke(impl);

        var request = impl.Request;

        return await Create(request, cancellationToken);
    }

    public async Task<Result> Create(CreateAlertRequest request, CancellationToken cancellationToken = default)
    {
        string url = "https://api.opsgenie.com/v2/alerts";
        
        return new SuccessfulResult {DebugInfo = new DebugInfo {URL = url, Request = request.ToJsonString()}};
    }


    class AlertDefinitionImpl :
        AlertDefinition
    {
        private string _description;
        private string _clientIdentifier;
        private string _notes;
        private List<string> _actions;
        private List<string> _tags;
        private string _domain;
        private AlertPriority _priority;
        private object[] _responderRecipients;
        private object[] _visibility;
        private IDictionary<string, string> _properties;

        public CreateAlertRequest Request =>
            new()
            {
                Description = _description,
                Responders = _responderRecipients,
                VisibleTo = _visibility,
                Alias = _clientIdentifier,
                Note = _notes,
                Details = _properties,
                Actions = _actions,
                Tags = _tags,
                Entity = _domain,
                Priority = _priority
            };

        public void Description(string description)
        {
            _description = description;
        }

        public void Responders(Action<Responder> action)
        {
            var impl = new ResponderImpl();
            
            action?.Invoke(impl);

            _responderRecipients = impl.Recipients.ToArray();
        }

        public void VisibleTo(Action<VisibleTo> action)
        {
            var impl = new VisibleToImpl();
            
            action?.Invoke(impl);

            _visibility = impl.AlertVisibility.ToArray();
        }

        public void ClientIdentifier(string alias)
        {
            _clientIdentifier = alias;
        }

        public void AdditionalNotes(string notes)
        {
            _notes = notes;
        }

        public void CustomProperties(Action<AlertProperty> action)
        {
            var impl = new AlertPropertyImpl();
            
            action?.Invoke(impl);

            _properties = impl.Properties;
        }

        public void CustomActions(string action, params string[] actions)
        {
            var temp = new List<string> {action};

            for (int i = 0; i < actions.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(actions[i]))
                    temp.Add(actions[i]);
            }

            _actions = temp;
        }

        public void CustomTags(string tag, params string[] tags)
        {
            var temp = new List<string> {tag};

            for (int i = 0; i < tags.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(tags[i]))
                    temp.Add(tags[i]);
            }

            _tags = temp;
        }

        public void RelatedToDomain(string domain)
        {
            _domain = domain;
        }

        public void Priority(AlertPriority priority)
        {
            _priority = priority;
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
        
        class ResponderImpl :
            Responder
        {
            public List<object> Recipients { get; }

            public ResponderImpl()
            {
                Recipients = new List<object>();
            }

            public void Team(Action<RespondToTeam> action)
            {
                var impl = new RespondToTeamImpl();
            
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            public void User(Action<RespondToUser> action)
            {
                var impl = new RespondToUserImpl();
            
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            public void Escalation(Action<RespondToEscalation> action)
            {
                var impl = new RespondToEscalationImpl();
            
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            public void Schedule(Action<RespondToSchedule> action)
            {
                var impl = new RespondToScheduleImpl();
            
                action?.Invoke(impl);

                Recipients.Add(impl.Data);
            }

            class RespondToEscalationImpl :
                RespondToEscalation
            {
                public object Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = Recipient.Add(id, RecipientType.Escalation);
                }

                public void Name(string name)
                {
                    Data = TeamRecipient.Add(name, RecipientType.Escalation);
                }
            }

            class RespondToScheduleImpl :
                RespondToSchedule
            {
                public object Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = Recipient.Add(id, RecipientType.Schedule);
                }

                public void Name(string name)
                {
                    Data = TeamRecipient.Add(name, RecipientType.Schedule);
                }
            }

            class RespondToUserImpl :
                RespondToUser
            {
                public object Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = Recipient.Add(id, RecipientType.User);
                }

                public void Username(string username)
                {
                    Data = UserRecipient.Add(username, RecipientType.User);
                }
            }

            class RespondToTeamImpl :
                RespondToTeam
            {
                public object Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = Recipient.Add(id, RecipientType.Team);
                }

                public void Name(string name)
                {
                    Data = TeamRecipient.Add(name, RecipientType.Team);
                }
            }
        }

        class VisibleToImpl :
            VisibleTo
        {
            public List<object> AlertVisibility { get; }

            public VisibleToImpl()
            {
                AlertVisibility = new List<object>();
            }

            public void Team(Action<VisibleToTeam> action)
            {
                var impl = new VisibleToTeamImpl();
            
                action?.Invoke(impl);

                AlertVisibility.Add(impl.Data);
            }

            public void User(Action<VisibleToUser> action)
            {
                var impl = new VisibleToUserImpl();
            
                action?.Invoke(impl);

                AlertVisibility.Add(impl.Data);
            }

            
            class VisibleToTeamImpl :
                VisibleToTeam
            {
                public object Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = Recipient.Add(id, RecipientType.Team);
                }

                public void Name(string name)
                {
                    Data = TeamRecipient.Add(name, RecipientType.Team);
                }
            }

            class VisibleToUserImpl :
                VisibleToUser
            {
                public object Data { get; private set; }
                
                public void Id(Guid id)
                {
                    Data = Recipient.Add(id, RecipientType.User);
                }

                public void Username(string username)
                {
                    Data = UserRecipient.Add(username, RecipientType.User);
                }
            }
        }
    }
}