namespace PetFamily.Application.Volunteers.AddPet;

public record AddressDto(
    string ZipCode,
    string Country,
    string State,
    string City,
    string StreetName,
    string StreetNumber,
    string Apartment);