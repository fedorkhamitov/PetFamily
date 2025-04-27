using CSharpFunctionalExtensions;

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

    private Address()
    {
    }

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

    public static Result<Address, Error> Create(
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
            return Errors.General.ValueIsRequired("ZipCode");
        if (!zipCode.All(char.IsDigit))
            return Errors.General.ValueIsInvalid("ZipCode");
        if (string.IsNullOrWhiteSpace(country))
            return Errors.General.ValueIsRequired("Country");
        if (country.All(char.IsLetter))
            return Errors.General.ValueIsInvalid("Country");
        if (string.IsNullOrWhiteSpace(state))
            return Errors.General.ValueIsRequired("State");
        if (state.All(char.IsLetter))
            return Errors.General.ValueIsInvalid("State");
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsRequired("City");
        if (city.All(char.IsLetter))
            return Errors.General.ValueIsInvalid("City");
        if (string.IsNullOrWhiteSpace(streetName))
            return Errors.General.ValueIsRequired("StreetName");
        if (string.IsNullOrWhiteSpace(streetNumber))
            return Errors.General.ValueIsRequired("StreetNumber");
        if (!streetNumber.All(char.IsDigit))
            return Errors.General.ValueIsInvalid("StreetNumber");
        if (string.IsNullOrWhiteSpace(apartment))
            return Errors.General.ValueIsInvalid("Apartment");
        if (!apartment.All(char.IsDigit))
            return Errors.General.ValueIsInvalid("Apartment");
        
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