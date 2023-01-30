using Flare.Extensions;
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
    public void Test1()
    {
        var expected = new CreateAlertRequest
        {
            Description = "Every alert needs a description",
            Responders = new object[]
            {
                Recipient.Add(NewId.NextGuid(), RecipientType.Team),
                TeamRecipient.Add("NOC", RecipientType.Team),
                Recipient.Add(NewId.NextGuid(), RecipientType.User),
                UserRecipient.Add("trinity@opsgenie.com", RecipientType.User),
                Recipient.Add(NewId.NextGuid(), RecipientType.Escalation),
                TeamRecipient.Add("Nightwatch Escalation", RecipientType.Escalation),
                Recipient.Add(NewId.NextGuid(), RecipientType.Schedule),
                TeamRecipient.Add("First Responders Schedule", RecipientType.Schedule)
            },
            VisibleTo = new object[]
            {
                Recipient.Add(NewId.NextGuid(), RecipientType.Team),
                TeamRecipient.Add("rocket_team", RecipientType.Team),
                Recipient.Add(NewId.NextGuid(), RecipientType.User),
                TeamRecipient.Add("trinity@opsgenie.com", RecipientType.User)
            },
            Alias = "Life is too short for no alias",
            Note = "Some fake notes here",
            Details = new Dictionary<string, string> {{"key1", "value1"}, {"key2", "value2"}},
            Actions = new List<string> {"Restart", "AnExampleAction"},
            Tags = new List<string> {"OverwriteQuietHours", "Critical"},
            Entity = "Fake entity",
            Priority = AlertPriority.P1
        };
        
        Console.WriteLine(expected.ToJsonString(Deserializer.Options));
        var actual = expected.ToJsonString(Deserializer.Options).ToObject<CreateAlertRequest>();
        
        // Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Test2()
    {
        var expected = new AlertResponse
        {
            Result = "Request will be processed",
            Took = 0.302f,
            RequestId = NewId.NextGuid()
        };

        var actual = expected.ToJsonString(Deserializer.Options).ToObject<AlertResponse>();
        
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
        
        Console.WriteLine(expected.ToJsonString(Deserializer.Options));
        // var actual = expected.ToJsonString(Deserializer.Options).ToObject<AlertDefinitionRequest>();
        
        // Assert.That(actual, Is.EqualTo(expected));
    }
}