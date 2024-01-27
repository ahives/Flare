namespace Flare.Alert;

public interface DeleteAlertCustomPropertiesCriteria
{
    void Details(Action<CustomPropertyKeyBuilder> action);

    void User(string displayName);

    void Source(string displayName);

    void Notes(string notes);
}