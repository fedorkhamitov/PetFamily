namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public record UpdateSocialNetworksRequest(Guid Id, IEnumerable<UpdateSocialNetworksDto> Dtos);