using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Share;

public record PetFile
{
    public FilePath PathToStorage { get; }

    [JsonConstructor]
    private PetFile(FilePath pathToStorage)
    {
        PathToStorage = pathToStorage;
    }

    public static Result<PetFile, Error> Create(FilePath path)
    {
        if (string.IsNullOrWhiteSpace(path.Path))
            return Errors.General.ValueIsRequired("Path");
        return new PetFile(path);
    }
}