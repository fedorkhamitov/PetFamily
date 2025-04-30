using CSharpFunctionalExtensions;
using PetFamily.Domain.Extensions;

namespace PetFamily.Domain.Infrastructure;

public record HumanName
{
    public string FirstName { get; } = default!;
    public string SecondName { get; } = default!;
    public string LastName { get; } = default!;

    private HumanName()
    { }

    private HumanName(string firstName, string secondName, string lastName)
    {
        FirstName = firstName;
        SecondName = secondName;
        LastName = lastName;
    }

    public static Result<HumanName, Error> Create(string firstName, string secondName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(secondName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            !firstName.IsOnlyWords() ||
            !secondName.IsOnlyWords() ||
            !lastName.IsOnlyWords()
           )
        {
            return Errors.General.ValueIsRequired("HumanName fields");
        }

        return new HumanName(firstName, secondName, lastName);
    }
}