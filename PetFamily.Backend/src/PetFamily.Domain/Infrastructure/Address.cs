using CSharpFunctionalExtensions;
using System.Linq;

namespace PetFamily.Domain.Infrastructure;

public record Address
{
    public string ZipCode { get; }
    public string Country { get; }
    public string State { get; }
    public string City { get; }
    public string StreetName { get; }
    public string StreetNumber { get; }
    public string Apartment { get; }

    private Address(
        string zipCode,
        string country,
        string state,
        string city,
        string streetName,
        string streetNumber,
        string apartment
    )
    {
        ZipCode = zipCode;
        Country = country;
        State = state;
        City = city;
        StreetName = streetName;
        StreetNumber = streetNumber;
        Apartment = apartment;
    }

    public static Result<Address> Create(
        string zipCode,
        string country,
        string state,
        string city,
        string streetName,
        string streetNumber,
        string apartment
        )
    {
        if (string.IsNullOrWhiteSpace(zipCode))
            return Result.Failure<Address>("Zip Code cannot be empty");
        if (!zipCode.All(char.IsDigit))
            return Result.Failure<Address>("Zip Code cannot be words");
        if (string.IsNullOrWhiteSpace(country))
            return Result.Failure<Address>("Country cannot be empty");
        if (country.All(char.IsLetter))
            return Result.Failure<Address>("Country cannot be numbers");
        if (string.IsNullOrWhiteSpace(state))
            return Result.Failure<Address>("State cannot be empty");
        if (state.All(char.IsLetter))
            return Result.Failure<Address>("State cannot be numbers");
        if (string.IsNullOrWhiteSpace(city))
            return Result.Failure<Address>("City cannot be empty");
        if (city.All(char.IsLetter))
            return Result.Failure<Address>("City cannot be numbers");
        if (string.IsNullOrWhiteSpace(streetName))
            return Result.Failure<Address>("Street name cannot be empty");
        if (string.IsNullOrWhiteSpace(streetNumber))
            return Result.Failure<Address>("Street Number cannot be empty");
        if (!streetNumber.All(char.IsDigit))
            return Result.Failure<Address>("Street Number cannot be words");
        if (string.IsNullOrWhiteSpace(apartment))
            return Result.Failure<Address>("Apartment cannot be empty");
        if (!apartment.All(char.IsDigit))
            return Result.Failure<Address>("Apartment cannot be words");
        
        return new Address(
            zipCode,
            country,
            state,
            city,
            streetName,
            streetNumber,
            apartment
        );
    }
}