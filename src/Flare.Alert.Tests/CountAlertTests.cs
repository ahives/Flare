namespace Flare.Alert.Tests;

using Flare.Model;
using MassTransit;
using Model;

[TestFixture]
public class CountAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder("TestData/AlertCountResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
                x.SearchIdentifierType(identifierType);
            });

        string searchIdentifierType = identifierType switch
        {
            IdentifierType.Id => "id",
            IdentifierType.Name => "name",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result!.Data.Count, Is.EqualTo(7));
            Assert.That(result.Result.Took, Is.EqualTo(0.051f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("9ae63dd7-ed00-4c81-86f0-c4ffd33142c9")));
            Assert.That(result.DebugInfo!.URL, Is.EqualTo($"alerts/count?searchIdentifier={identifier}&searchIdentifierType={searchIdentifierType}"));
        });
    }

    [Test]
    public async Task Test2()
    {
        string identifier = "openAlerts";
        var identifierType = IdentifierType.Name;
        var result = await GetContainerBuilder("TestData/AlertCountResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
                x.SearchIdentifierType(identifierType);
            });

        string searchIdentifierType = identifierType switch
        {
            IdentifierType.Id => "id",
            IdentifierType.Name => "name",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result!.Data.Count, Is.EqualTo(7));
            Assert.That(result.Result.Took, Is.EqualTo(0.051f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("9ae63dd7-ed00-4c81-86f0-c4ffd33142c9")));
            Assert.That(result.DebugInfo!.URL, Is.EqualTo($"alerts/count?searchIdentifier={identifier}&searchIdentifierType={searchIdentifierType}"));
        });
    }

    [Test]
    public async Task Test3()
    {
        string identifier = "openAlerts";
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder("TestData/AlertCountResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
                x.SearchIdentifierType(identifierType);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.Result, Is.Null);
        });
    }

    [Test]
    public async Task Test5()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.Name;
        var status = AlertStatus.Open;
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
                x.SearchIdentifierType(identifierType);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(1));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeIncompatible));
        });
    }

    [Test]
    public async Task Test6()
    {
        string identifier = string.Empty;
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
                x.SearchIdentifierType(identifierType);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(2));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeIncompatible));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierInvalid));
        });
    }

    [Test]
    public async Task Test7()
    {
        string identifier = string.Empty;
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(3));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeIncompatible));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeMissing));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierInvalid));
        });
    }

    [Test]
    public async Task Test8()
    {
        string identifier = string.Empty;
        var result = await GetContainerBuilder()
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(identifier);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo!.Errors.Count, Is.EqualTo(2));
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.IdentifierTypeMissing));
        });
    }
}