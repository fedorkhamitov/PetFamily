using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Pets.DeletePetFiles;
using PetFamily.Application.Pets.UploadPetFiles;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;
using PetFamily.Application.Volunteers.UpdateDonationDetails;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<UpdateDonationDetailsHandler>();
        services.AddScoped<HardDeleteHandler>();
        services.AddScoped<SoftDeleteHandler>();
        services.AddScoped<RestoreHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<UploadPetFileHandler>();
        services.AddScoped<DeletePetFilesHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}