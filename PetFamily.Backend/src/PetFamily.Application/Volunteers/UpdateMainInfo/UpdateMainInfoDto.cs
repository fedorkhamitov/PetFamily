using PetFamily.Application.Volunteers.Create;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoDto(
    HumanNameRequestDto Name, 
    string Email,
    string Description,
    int YearsOfWorkExp,
    string PhoneNumber);