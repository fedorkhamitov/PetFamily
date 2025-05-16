using CSharpFunctionalExtensions;
using System.Text.Json.Serialization;

namespace PetFamily.Domain.Share;

public record FilePath
{
    public string Path { get; }

    [JsonConstructor]
    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath, Error> Create(string path, string extension = "")
    {
        if (string.IsNullOrWhiteSpace(path.ToString()))
            return Errors.General.ValueIsRequired("File path");
        return new FilePath(path + extension);
    }
}