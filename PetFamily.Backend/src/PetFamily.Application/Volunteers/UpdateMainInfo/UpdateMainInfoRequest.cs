using PetFamily.Application.Volunteers.Create;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(Guid Id, UpdateMainInfoDto Dto);