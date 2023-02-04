using Flare.Configuration;
using Flare.Model;
using MassTransit;

namespace Flare.Alert.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    // [Test]
    // public void Test1()
    // {
    //     FlareConfigProvider provider = new FlareConfigProvider();
    //     var config = provider.Configure(p =>
    //     {
    //         p.Client(c =>
    //         {
    //             c.ConnectTo("");
    //             c.UsingApiKey("");
    //         });
    //     });
    //
    //     // var alert = new AlertImpl(config);
    //     FlareClient client = new FlareClient(config);
    //     var result = client.API<Alert>()
    //         .Create(x =>
    //         {
    //             x.Description("Every alert needs a description");
    //             x.Responders(r =>
    //             {
    //                 r.Team(o => o.Id(NewId.NextGuid()));
    //                 r.Team(o => o.Name("NOC"));
    //                 r.User(o => o.Id(NewId.NextGuid()));
    //                 r.User(o => o.Username("trinity@opsgenie.com"));
    //                 r.Escalation(o => o.Id(NewId.NextGuid()));
    //                 r.Escalation(o => o.Name("Nightwatch Escalation"));
    //                 r.Schedule(o => o.Id(NewId.NextGuid()));
    //                 r.Schedule(o => o.Name("First Responders Schedule"));
    //             });
    //             x.VisibleTo(v =>
    //             {
    //                 v.Team(o => o.Id(NewId.NextGuid()));
    //                 v.Team(o => o.Name("rocket_team"));
    //                 v.User(o => o.Id(NewId.NextGuid()));
    //                 v.User(o => o.Username("trinity@opsgenie.com"));
    //             });
    //             x.ClientIdentifier("Life is too short for no alias");
    //             x.AdditionalNotes("Some fake notes here");
    //             x.CustomProperties(p =>
    //             {
    //                 p.Add("key1", "value1");
    //                 p.Add("key2", "value2");
    //             });
    //             x.CustomActions("Restart", "AnExampleAction");
    //             x.CustomTags("OverwriteQuietHours", "Critical");
    //             x.RelatedToDomain("Fake entity");
    //             x.Priority(AlertPriority.P1);
    //         });
    //     
    //     Console.WriteLine(result.DebugInfo.Request);
    //     // Console.WriteLine(result.DebugInfo.Request.ToJsonString(Deserializer.Options));
    //     // var actual = expected.ToJsonString(Deserializer.Options).ToObject<CreateAlertRequest>();
    //     
    //     // Assert.That(actual, Is.EqualTo(expected));
    // }
}