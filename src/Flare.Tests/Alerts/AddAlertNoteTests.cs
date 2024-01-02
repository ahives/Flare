namespace Flare.Tests.Alerts;

using Microsoft.Extensions.DependencyInjection;

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
            .AddNote(x =>
            {
                x.Definition(d =>
                {
                    d.User("Monitoring Script");
                    d.Source("AWS Lambda");
                    d.Note("Action executed via Alert API");
                });
                x.Where(q =>
                {
                    q.SearchIdentifier(NewId.NextGuid());
                    q.SearchIdentifierType(IdentifierType.Id);
                });
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
}