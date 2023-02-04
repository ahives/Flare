using Flare.Configuration;
using Flare.Extensions;
using Flare.Model;
using Flare.Serialization;
using MassTransit;

namespace Flare.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var config = new FlareConfigProvider()
            .Configure(p =>
            {
                p.Client(c =>
                {
                    c.ConnectTo("");
                    c.UsingApiKey("");
                    c.UsingCredentials("guest", "guest");
                });
            });

        var result = await new FlareClient(config)
            .API<Alert>()
            .Create(x =>
            {
                x.Description("Every alert needs a description");
                x.Responders(r =>
                {
                    r.Team(o => o.Id(NewId.NextGuid()));
                    r.Team(o => o.Name("NOC"));
                    r.User(o => o.Id(NewId.NextGuid()));
                    r.User(o => o.Username("trinity@opsgenie.com"));
                    r.Escalation(o => o.Id(NewId.NextGuid()));
                    r.Escalation(o => o.Name("Nightwatch Escalation"));
                    r.Schedule(o => o.Id(NewId.NextGuid()));
                    r.Schedule(o => o.Name("First Responders Schedule"));
                });
                x.VisibleTo(v =>
                {
                    v.Team(o => o.Id(NewId.NextGuid()));
                    v.Team(o => o.Name("rocket_team"));
                    v.User(o => o.Id(NewId.NextGuid()));
                    v.User(o => o.Username("trinity@opsgenie.com"));
                });
                x.ClientIdentifier("Life is too short for no alias");
                x.AdditionalNotes("Some fake notes here");
                x.CustomProperties(p =>
                {
                    p.Add("key1", "value1");
                    p.Add("key2", "value2");
                });
                x.CustomActions("Restart", "AnExampleAction");
                x.CustomTags("OverwriteQuietHours", "Critical");
                x.RelatedToDomain("Fake entity");
                x.Priority(AlertPriority.P1);
            });
        
        Console.WriteLine(result.DebugInfo.Request);
        // Console.WriteLine(result.DebugInfo.Request.ToJsonString(Deserializer.Options));
        // var actual = expected.ToJsonString(Deserializer.Options).ToObject<CreateAlertRequest>();
        
        // Assert.That(actual, Is.EqualTo(expected));
    }

    // [Test]
    // public void Test1b()
    // {
    //     var expected = new CreateAlertRequest
    //     {
    //         Description = "Every alert needs a description",
    //         Responders = new object[]
    //         {
    //             Recipient.Add(NewId.NextGuid(), RecipientType.Team),
    //             TeamRecipient.Add("NOC", RecipientType.Team),
    //             Recipient.Add(NewId.NextGuid(), RecipientType.User),
    //             UserRecipient.Add("trinity@opsgenie.com", RecipientType.User),
    //             Recipient.Add(NewId.NextGuid(), RecipientType.Escalation),
    //             TeamRecipient.Add("Nightwatch Escalation", RecipientType.Escalation),
    //             Recipient.Add(NewId.NextGuid(), RecipientType.Schedule),
    //             TeamRecipient.Add("First Responders Schedule", RecipientType.Schedule)
    //         },
    //         VisibleTo = new object[]
    //         {
    //             Recipient.Add(NewId.NextGuid(), RecipientType.Team),
    //             TeamRecipient.Add("rocket_team", RecipientType.Team),
    //             Recipient.Add(NewId.NextGuid(), RecipientType.User),
    //             TeamRecipient.Add("trinity@opsgenie.com", RecipientType.User)
    //         },
    //         Alias = "Life is too short for no alias",
    //         Note = "Some fake notes here",
    //         Details = new Dictionary<string, string> {{"key1", "value1"}, {"key2", "value2"}},
    //         Actions = new List<string> {"Restart", "AnExampleAction"},
    //         Tags = new List<string> {"OverwriteQuietHours", "Critical"},
    //         Entity = "Fake entity",
    //         Priority = AlertPriority.P1
    //     };
    //     
    //     // Console.WriteLine(result.DebugInfo.Request.ToJsonString(Deserializer.Options));
    //     // var actual = expected.ToJsonString(Deserializer.Options).ToObject<CreateAlertRequest>();
    //     
    //     // Assert.That(actual, Is.EqualTo(expected));
    // }

    [Test]
    public void Test2()
    {
        var expected = new AlertResponse
        {
            Result = "Request will be processed",
            Took = 0.302f,
            RequestId = NewId.NextGuid()
        };

        var actual = expected.ToJsonString(Serializer.Options).ToObject<AlertResponse>();
        
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test3()
    {
        var expected = new AlertData
        {
            Id = NewId.NextGuid(),
            TinyId = "1791",
            Alias = "event_573",
            Message = "Our servers are in danger",
            Status = AlertStatus.Closed,
            Acknowledged = false,
            IsSeen = true,
            Tags = new List<string> {"OverwriteQuietHours", "Critical"},
            Snoozed = true,
            SnoozedUntil = DateTimeOffset.UtcNow,
            Count = 79,
            LastOccurredAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Source = "Isengard",
            Owner = "morpheus@opsgenie.com",
            Priority = AlertPriority.P5,
            Responders = new object[]
            {
                Recipient.Add(NewId.NextGuid(), RecipientType.Team),
                Recipient.Add(NewId.NextGuid(), RecipientType.User),
                Recipient.Add(NewId.NextGuid(), RecipientType.Escalation),
                Recipient.Add(NewId.NextGuid(), RecipientType.Schedule)
            },
            Integration = new ApiIntegration
            {
                Id = NewId.NextGuid(),
                Name = "Nebuchadnezzar",
                Type = ApiIntegrationType.API
            },
            Report = new AlertReport
            {
                AckTime = 15702,
                CloseTime = 60503,
                AcknowledgedBy = "agent_smith@opsgenie.com",
                ClosedBy = "neo@opsgenie.com"
            },
            Actions = new List<string> {"Restart", "Ping"},
            Entity = "EC2",
            Description = "Example description",
            Details = new AlertDetails
            {
                ServerName = "Zion",
                Region = "Oregon"
            }
        };
        
        Console.WriteLine(expected.ToJsonString(Serializer.Options));
        // var actual = expected.ToJsonString(Deserializer.Options).ToObject<AlertDefinitionRequest>();
        
        // Assert.That(actual, Is.EqualTo(expected));
    }
}