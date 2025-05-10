using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Share;

public record Position
{
    public int Value { get; }

    private Position(int value)
    {
        Value = value;
    }

    public static Result<Position, Error> Create(int value)
    {
        if (value < 1)
            return Errors.General.ValueIsInvalid("Serial Number");
        return new Position(value);
    }

    public static Position First => new(1);

    public Result<Position, Error> Forward() => Create(Value + 1);
    public Result<Position, Error> Back() => Value < 1 ? 
        Errors.General.ValueIsInvalid("Value") : Create(Value - 1);
}