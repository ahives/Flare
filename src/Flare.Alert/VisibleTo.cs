namespace Flare.Alert;

public interface VisibleTo
{
    void Team(Action<VisibleToTeam> action);

    void User(Action<VisibleToUser> action);
}