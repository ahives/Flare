namespace Flare.Tests;

[TestFixture]
public class CloseAlertTests
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
            .Close(NewId.NextGuid(), x =>
                {
                    x.User("Flare");
                    x.Source("Flare");
                    x.Note("");
                },
                q =>
                {
                    q.SearchIdentifierType(CloseSearchIdentifierType.Id);
                });
        
        Console.WriteLine(result.DebugInfo.URL);
        Console.WriteLine(result.DebugInfo.Request);
    }
}