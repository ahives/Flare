namespace Flare.Tests;

[TestFixture]
public class AddAlertNoteTests
{
    FlareConfig _config;

    [SetUp]
    public void Setup()
    {
        _config = new FlareConfigProvider()
            .Configure(p =>
            {
                p.Client(c =>
                {
                    c.ConnectTo("");
                    c.UsingApiKey("");
                    c.UsingCredentials("guest", "guest");
                });
            });
    }

    [Test]
    public async Task Test1()
    {
        var result = await new FlareClient(_config)
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
                    q.Identifier(NewId.NextGuid());
                    q.IdentifierType(AddAlertNoteIdentifierType.Id);
                });
            });

        Console.WriteLine(result.DebugInfo.URL);
        Console.WriteLine(result.DebugInfo.Request);
    }
}