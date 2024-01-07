namespace Flare.Alert.Tests;

using Flare.Model;
using MassTransit;

[TestFixture]
public class AddAlertNoteTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/AddAlertNoteResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .AddNote(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.User("Monitoring Script");
                x.Source("AWS Lambda");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.213f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }

    [Test]
    public async Task Test2()
    {
        var result = await GetContainerBuilder("TestData/AddAlertNoteResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .AddNote(NewId.NextGuid().ToString(), IdentifierType.Name, x =>
            {
                x.User("Monitoring Script");
                x.Source("AWS Lambda");
                x.Note("Action executed via Alert API");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.HasFaulted, Is.True);
        });
    }
}