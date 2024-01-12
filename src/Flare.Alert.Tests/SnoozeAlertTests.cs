namespace Flare.Alert.Tests;

using Flare.Model;
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
                x.Notes("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result!.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.109f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test2()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.User("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.StringLengthLimitExceeded));
        });
    }

    [Test]
    public async Task Test3()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.Source("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.StringLengthLimitExceeded));
        });
    }

    [Test]
    public async Task Test4()
    {
        string notes = await File.ReadAllTextAsync($"{TestContext.CurrentContext.TestDirectory}/TestData/long_message.txt");
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.Notes(notes);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.StringLengthLimitExceeded));
        });
    }

    [Test]
    public async Task Test5()
    {
        string notes = await File.ReadAllTextAsync($"{TestContext.CurrentContext.TestDirectory}/TestData/long_message.txt");
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(NewId.NextGuid().ToString(), IdentifierType.Name, x =>
            {
                x.User("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                x.Source("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                x.Notes(notes);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(6));
            Assert.That(result.DebugInfo.Errors.Count(x => x.Type == ErrorType.StringLengthLimitExceeded), Is.EqualTo(3));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeInvalidWithinContext));
        });
    }

    [Test]
    public async Task Test6()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze(Guid.Empty.ToString(), IdentifierType.Id, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.User("Flare");
                x.Source("Flare");
                x.Notes("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierInvalid));
        });
    }

    [Test]
    public async Task Test7()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Snooze("no_alias", IdentifierType.Id, x =>
            {
                x.EndTime(DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"));
                x.User("Flare");
                x.Source("Flare");
                x.Notes("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeIncompatible));
        });
    }
}