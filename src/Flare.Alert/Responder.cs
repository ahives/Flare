namespace Flare.Alert;

public interface Responder
{
    void Team(Action<TeamResponder> action);

    void User(Action<UserResponder> action);
    
    void Escalation(Action<EscalationResponder> action);
    
    void Schedule(Action<ScheduleResponder> action);
}