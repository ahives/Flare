namespace Flare.Alert.Tests;

using MassTransit;

[TestFixture]
public class SnoozeAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/SnoozeAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.User("Flare");
                x.Source("Flare");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.109f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test2()
    {
        var result = await GetContainerBuilder("TestData/SnoozeAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Name, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.User("Flare");
                x.Source("Flare");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo, Is.Not.Null);
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierType), Is.True);
        });
    }

    [Test]
    public async Task Test3()
    {
        var result = await GetContainerBuilder("TestData/SnoozeAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.User("Flare");
                x.Source("Flare");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo, Is.Not.Null);
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.EndTime), Is.True);
        });
    }
}