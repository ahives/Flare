namespace Flare.Alert;

public interface AddAlertCustomPropertiesCriteria
{
    void Details(Action<CustomPropertyBuilder> action);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}