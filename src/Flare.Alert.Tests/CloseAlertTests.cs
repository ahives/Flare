namespace Flare.Alert.Tests;

using Flare.Model;
using MassTransit;

[TestFixture]
public class CloseAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder("TestData/CloseAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Close(identifier, identifierType, x =>
            {
                x.User("Monitoring Script");
                x.Source("AWS Lambda");
                x.Note("Action executed via Alert API");
            });

        string identifierTypeString = identifierType switch
        {
            IdentifierType.Id => "id",
            IdentifierType.Tiny => "tiny",
            IdentifierType.Alias => "alias",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts/{identifier}/close?identifierType={identifierTypeString}"));
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.107f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test2()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder("TestData/CloseAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Close(identifier, identifierType, x =>
            {
                x.User("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
                x.Source("AWS Lambda");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.StringLengthLimitExceeded), Is.True);
        });
    }

    [Test]
    public async Task Test3()
    {
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder("TestData/CloseAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Close(identifier, identifierType, x =>
            {
                x.User("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
                x.Source("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.HasFaulted, Is.True);
            Assert.That(result.DebugInfo.Errors.Any(x => x.Type == ErrorType.StringLengthLimitExceeded), Is.True);
            Assert.That(result.DebugInfo.Errors.Count(x => x.Type == ErrorType.StringLengthLimitExceeded), Is.EqualTo(2));
        });
    }
}