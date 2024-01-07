namespace Flare.Alert;

public interface DeleteAlertCriteria
{
    void User(string displayName);

    void Source(string displayName);
}