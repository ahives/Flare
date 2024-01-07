namespace Flare.Alert.Tests;

using MassTransit;

public class DeleteAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.AlertId;
        var result = await GetContainerBuilder("TestData/DeleteAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Delete(identifier, identifierType, x =>
            {
                x.Source("");
                x.User("");
            });

        string identifierTypeString = identifierType switch
        {
            IdentifierType.AlertId => "AlertID",
            IdentifierType.TinyId => "tinyID",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts/{identifier}?identifierType={identifierTypeString}"));
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.204f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test2()
    {
        string identifier = NewId.NextGuid().ToString();
        string source = "test_source";
        var identifierType = IdentifierType.AlertId;
        var result = await GetContainerBuilder("TestData/DeleteAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Delete(identifier, identifierType, x =>
            {
                x.Source(source);
                x.User("");
            });

        string identifierTypeString = identifierType switch
        {
            IdentifierType.AlertId => "AlertID",
            IdentifierType.TinyId => "tinyID",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts/{identifier}?identifierType={identifierTypeString}&source={source}"));
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.204f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test3()
    {
        string identifier = NewId.NextGuid().ToString();
        string user = "test_user";
        var identifierType = IdentifierType.AlertId;
        var result = await GetContainerBuilder("TestData/DeleteAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Delete(identifier, identifierType, x =>
            {
                x.Source("");
                x.User(user);
            });

        string identifierTypeString = identifierType switch
        {
            IdentifierType.AlertId => "AlertID",
            IdentifierType.TinyId => "tinyID",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts/{identifier}?identifierType={identifierTypeString}&user={user}"));
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.204f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test4()
    {
        string identifier = NewId.NextGuid().ToString();
        string source = "test_source";
        string user = "test_user";
        var identifierType = IdentifierType.AlertId;
        var result = await GetContainerBuilder("TestData/DeleteAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Delete(identifier, identifierType, x =>
            {
                x.Source(source);
                x.User(user);
            });

        string identifierTypeString = identifierType switch
        {
            IdentifierType.AlertId => "AlertID",
            IdentifierType.TinyId => "tinyID",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts/{identifier}?identifierType={identifierTypeString}&user={user}&source={source}"));
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.204f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test5()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.AlertId;
        var result = await GetContainerBuilder("TestData/DeleteAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Delete(identifier, identifierType, x =>
            {
                x.Source("");
                x.User("");
            });

        string identifierTypeString = identifierType switch
        {
            IdentifierType.AlertId => "AlertID",
            IdentifierType.TinyId => "tinyID",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts/{identifier}?identifierType={identifierTypeString}"));
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.204f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }
}