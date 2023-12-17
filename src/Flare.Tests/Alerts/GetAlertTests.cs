namespace Flare.Tests.Alerts;

using Extensions;
using Model;
using Microsoft.Extensions.DependencyInjection;

[TestFixture]
public class GetAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test()
    {
        var result = await GetContainerBuilder("TestData/GetAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()
            .API<Alert>()
            .Get(NewId.NextGuid().ToString(), x => x.SearchIdentifierType(GetQuerySearchIdentifierType.Id));

        Assert.Multiple(() =>
        {
            Assert.That(result.HasData, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Data.Data.Id, Is.EqualTo(Guid.Parse("70413a06-38d6-4c85-92b8-5ebc900d42e2")));
            Assert.That(result.Data.Data.TinyId, Is.EqualTo("1791"));
            Assert.That(result.Data.Data.Alias, Is.EqualTo("event_573"));
            Assert.That(result.Data.Data.Message, Is.EqualTo("Our servers are in danger"));
            Assert.That(result.Data.Data.Status, Is.EqualTo(AlertStatus.Closed));
            Assert.That(result.Data.Data.Acknowledged, Is.False);
            Assert.That(result.Data.Data.IsSeen, Is.True);
            // Assert.That(result.Data.Data.Tags, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data.Tags.Any(x => x), Is.EqualTo("Request will be processed"));
            Assert.That(result.Data.Data.Snoozed, Is.True);
            Assert.That(result.Data.Data.SnoozedUntil, Is.EqualTo(DateTimeOffset.Parse("2017-04-03T20:32:35.143Z")));
            Assert.That(result.Data.Data.LastOccurredAt, Is.EqualTo(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z")));
            Assert.That(result.Data.Data.CreatedAt, Is.EqualTo(DateTimeOffset.Parse("2017-03-21T20:32:52.353Z")));
            Assert.That(result.Data.Data.UpdatedAt, Is.EqualTo(DateTimeOffset.Parse("2017-04-03T20:32:57.301Z")));
            Assert.That(result.Data.Data.Count, Is.EqualTo(79));
            Assert.That(result.Data.Data.Source, Is.EqualTo("Isengard"));
            Assert.That(result.Data.Data.Owner, Is.EqualTo("morpheus@opsgenie.com"));
            Assert.That(result.Data.Data.Priority, Is.EqualTo(AlertPriority.P5));
            Assert.That(result.Data.Data.Integration.Id,
                Is.EqualTo(Guid.Parse("4513b7ea-3b91-438f-b7e4-e3e54af9147c")));
            Assert.That(result.Data.Data.Integration.Name, Is.EqualTo("Nebuchadnezzar"));
            Assert.That(result.Data.Data.Integration.Type, Is.EqualTo(ApiIntegrationType.API));
            Assert.That(result.Data.Data.Report.AckTime, Is.EqualTo(15702));
            Assert.That(result.Data.Data.Report.CloseTime, Is.EqualTo(60503));
            Assert.That(result.Data.Data.Report.AcknowledgedBy, Is.EqualTo("agent_smith@opsgenie.com"));
            Assert.That(result.Data.Data.Report.ClosedBy, Is.EqualTo("neo@opsgenie.com"));
            Assert.That(result.Data.Data.Entity, Is.EqualTo("EC2"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
        });
        var request = result.DebugInfo.Request.ToObject<CreateAlertRequest>();
    }
}