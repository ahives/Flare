namespace Flare.Alert.Tests;

using Flare.Model;

[TestFixture]
public class AssignAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/AssignAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.Owner(e => e.Id(NewId.NextGuid()));
                x.User("Flare");
                x.Source("Flare");
                x.Notes("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result!.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.202f));
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
            .Assign(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.User("Flare");
                x.Source("Flare");
                x.Notes("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.OwnerMissing));
        });
    }

    [Test]
    public async Task Test3()
    {
        string notes = await File.ReadAllTextAsync($"{TestContext.CurrentContext.TestDirectory}/TestData/long_message.txt");
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.Owner(e => e.Id(NewId.NextGuid()));
                x.User("Flare");
                x.Source("Flare");
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
    public async Task Test4()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.Owner(e => e.Id(NewId.NextGuid()));
                x.User("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                x.Source("Flare");
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
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.Owner(e => e.Id(NewId.NextGuid()));
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
    public async Task Test6()
    {
        string notes = await File.ReadAllTextAsync($"{TestContext.CurrentContext.TestDirectory}/TestData/long_message.txt");
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.User("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                x.Source("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                x.Notes(notes);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(4));
            Assert.That(result.DebugInfo.Errors.Count(x => x.Type == ErrorType.StringLengthLimitExceeded), Is.EqualTo(3));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.OwnerMissing));
        });
    }

    [Test]
    public async Task Test7()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign(Guid.Empty.ToString(), IdentifierType.Id, x =>
            {
                x.Owner(e => e.Id(NewId.NextGuid()));
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
    public async Task Test8()
    {
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Assign("no_alias", IdentifierType.Id, x =>
            {
                x.Owner(e => e.Id(NewId.NextGuid()));
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