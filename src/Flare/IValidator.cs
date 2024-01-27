namespace Flare;

using Model;

public interface IValidator
{
    IReadOnlyList<Error> Validate();
}