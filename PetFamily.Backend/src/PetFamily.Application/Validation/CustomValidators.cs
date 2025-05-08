using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Domain.Share;

namespace PetFamily.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder, 
        Func<TElement, Result<TValueObject, Error>> factoryMethod) {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);
            if (result.IsSuccess) return;
            context.AddFailure(result.Error.Serialize());
        });
    }
    
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, string propertyName)
    {
        var error = Errors.General.ValueIsInvalid($"{propertyName}");
        return rule.WithMessage(error.Serialize());
    }
}