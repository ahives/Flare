namespace Flare.Tests.Alerts;

using Microsoft.Extensions.DependencyInjection;

[TestFixture]
public class AcknowledgeAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/AcknowledgeAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Acknowledge(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.User("Flare");
                x.Source("Flare");
                x.Note("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.302f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test2()
    {
        var result = await GetContainerBuilder("TestData/AcknowledgeAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Acknowledge(NewId.NextGuid().ToString(), IdentifierType.Name, x =>
            {
                x.User("Flare");
                x.Source("Flare");
                x.Note("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.HasFaulted, Is.True);
        });
    }

    // [Test]
    // public async Task Test2()
    // {
    //     var result = await new FlareClient(_config)
    //         .API<Alert>()
    //         .Acknowledge(NewId.NextGuid(), x =>
    //         {
    //             x.User("Flare");
    //             x.Source("Flare");
    //             x.Note("");
    //             x.SearchIdentifierType(AcknowledgeSearchIdentifierType.Id);
    //         });
    //     
    //     Console.WriteLine(result.DebugInfo.URL);
    //     Console.WriteLine(result.DebugInfo.Request);
    // }
}