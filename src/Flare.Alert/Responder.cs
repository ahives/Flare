namespace Flare.Alert;

public interface Responder
{
    void Team(Action<RespondToTeam> action);

    void User(Action<RespondToUser> action);
    
    void Escalation(Action<RespondToEscalation> action);
    
    void Schedule(Action<RespondToSchedule> action);
}