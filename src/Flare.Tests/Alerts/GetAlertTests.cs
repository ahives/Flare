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
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Get(NewId.NextGuid().ToString(), IdentifierType.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result.Data.Id, Is.EqualTo(Guid.Parse("70413a06-38d6-4c85-92b8-5ebc900d42e2")));
            Assert.That(result.Result.Data.TinyId, Is.EqualTo("1791"));
            Assert.That(result.Result.Data.Alias, Is.EqualTo("event_573"));
            Assert.That(result.Result.Data.Message, Is.EqualTo("Our servers are in danger"));
            Assert.That(result.Result.Data.Status, Is.EqualTo(AlertStatus.Closed));
            Assert.That(result.Result.Data.Acknowledged, Is.False);
            Assert.That(result.Result.Data.IsSeen, Is.True);
            // Assert.That(result.Data.Data.Tags, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data.Tags.Any(x => x), Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Data.Snoozed, Is.True);
            Assert.That(result.Result.Data.SnoozedUntil, Is.EqualTo(DateTimeOffset.Parse("2017-04-03T20:32:35.143Z")));
            Assert.That(result.Result.Data.LastOccurredAt, Is.EqualTo(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z")));
            Assert.That(result.Result.Data.CreatedAt, Is.EqualTo(DateTimeOffset.Parse("2017-03-21T20:32:52.353Z")));
            Assert.That(result.Result.Data.UpdatedAt, Is.EqualTo(DateTimeOffset.Parse("2017-04-03T20:32:57.301Z")));
            Assert.That(result.Result.Data.Count, Is.EqualTo(79));
            Assert.That(result.Result.Data.Source, Is.EqualTo("Isengard"));
            Assert.That(result.Result.Data.Owner, Is.EqualTo("morpheus@opsgenie.com"));
            Assert.That(result.Result.Data.Priority, Is.EqualTo(AlertPriority.P5));
            Assert.That(result.Result.Data.Integration.Id,
                Is.EqualTo(Guid.Parse("4513b7ea-3b91-438f-b7e4-e3e54af9147c")));
            Assert.That(result.Result.Data.Integration.Name, Is.EqualTo("Nebuchadnezzar"));
            Assert.That(result.Result.Data.Integration.Type, Is.EqualTo(ApiIntegrationType.API));
            Assert.That(result.Result.Data.Report.AckTime, Is.EqualTo(15702));
            Assert.That(result.Result.Data.Report.CloseTime, Is.EqualTo(60503));
            Assert.That(result.Result.Data.Report.AcknowledgedBy, Is.EqualTo("agent_smith@opsgenie.com"));
            Assert.That(result.Result.Data.Report.ClosedBy, Is.EqualTo("neo@opsgenie.com"));
            Assert.That(result.Result.Data.Entity, Is.EqualTo("EC2"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
            // Assert.That(result.Data.Data, Is.EqualTo("Request will be processed"));
        });
        var request = result.DebugInfo.Request.ToObject<CreateAlertRequest>();
    }
}