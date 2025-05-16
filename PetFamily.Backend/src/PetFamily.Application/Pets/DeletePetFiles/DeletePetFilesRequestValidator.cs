using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Pets.DeletePetFiles;

public class DeletePetFilesRequestValidator : AbstractValidator<DeletePetFilesRequest>
{
    public DeletePetFilesRequestValidator()
    {
        RuleForEach(r => r.FilePaths)
            .MustBeValueObject(fp => FilePath.Create(fp.Path));
    }
}