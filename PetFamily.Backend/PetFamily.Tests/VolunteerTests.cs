using FluentAssertions;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;
using PetFamily.Tests.TestInstances;

namespace PetFamily.Tests;

public class VolunteerTests
{
    [Fact]
    public void AddPet_First_Approach_Returns_Success_Result()
    {
        //arrange
        var volunteer = Instances.TestVolunteerInstance();

        var pet = Instances.TestPetInstance();
        
        //act
        var result = volunteer.AddPet(pet);

        //assert
        var petId = pet.Id;
        var addedPet = volunteer.GetPetById(petId);
        
        result.IsSuccess.Should().BeTrue();
        addedPet.IsSuccess.Should().BeTrue();
        addedPet.Value.Id.Should().Be(pet.Id);
        addedPet.Value.Position.Should().Be(Position.First);

    }

    [Fact]
    public void AddPet_Many_Returns_Success_Result()
    {
        //arrange
        var volunteer = Instances.TestVolunteerInstance();

        var pets = Enumerable.Range(1, 5).Select(_ => Instances.TestPetInstance());

        var position = 1;
        
        //act
        var result = pets.Select(pet => volunteer.AddPet(pet)).ToList();

        //assert
        result.Should().OnlyContain(r => r.IsSuccess);
        foreach (var pet in volunteer.Pets)
        {
            pet.Position.Value.Should().Be(position++);
        }
    }
    
    [Fact]
    public void Move_Pet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        // arrange
        var volunteer = Instances.TestVolunteerInstance();
            
        var pets = new List<Pet>(){ 
            Instances.TestPetInstance("Шарик"), 
            Instances.TestPetInstance("Бобик"),
            Instances.TestPetInstance("Тузик"),
            Instances.TestPetInstance("Мухтар"),
            Instances.TestPetInstance("Тобик"),
            Instances.TestPetInstance("Рекс") 
        };

        foreach (var pet in pets)
        {
            volunteer.AddPet(pet);
        }
        
        var currentPosition = Position.Create(5).Value;
        var newPosition = Position.Create(5).Value;

        var movingPet = volunteer.Pets.Single(p => p.Position == currentPosition);
        var nameOfMovingPet = movingPet.Name;
        // act
        var result = volunteer.MovePet(movingPet, newPosition);
        // assert
        result.IsSuccess.Should().BeTrue();
        movingPet.Position.Should().Be(newPosition);
        movingPet.Name.Should<string>().Be(nameOfMovingPet);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_When_New_Position_Is_Higher()
    {
        // arrange
        var volunteer = Instances.TestVolunteerInstance();
            
        var pets = new List<Pet>(){ 
            Instances.TestPetInstance("Шарик"), 
            Instances.TestPetInstance("Бобик"),
            Instances.TestPetInstance("Тузик"),
            Instances.TestPetInstance("Мухтар"),
            Instances.TestPetInstance("Тобик"),
            Instances.TestPetInstance("Рекс") 
        };

        foreach (var pet in pets)
        {
            volunteer.AddPet(pet);
        }

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        var sixthPet = volunteer.Pets[5];

        // act
        var result = volunteer.MovePet(secondPet, Position.Create(5).Value);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(5).Value);
        thirdPet.Position.Should().Be(Position.Create(2).Value);
        fourthPet.Position.Should().Be(Position.Create(3).Value);
        fifthPet.Position.Should().Be(Position.Create(4).Value);
        sixthPet.Position.Should().Be(Position.Create(6).Value);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_When_New_Position_Is_Lower()
    {
        // arrange
        var volunteer = Instances.TestVolunteerInstance();
            
        var pets = new List<Pet>(){ 
            Instances.TestPetInstance("Шарик"), 
            Instances.TestPetInstance("Бобик"),
            Instances.TestPetInstance("Тузик"),
            Instances.TestPetInstance("Мухтар"),
            Instances.TestPetInstance("Тобик"),
            Instances.TestPetInstance("Рекс") 
        };

        foreach (var pet in pets)
        {
            volunteer.AddPet(pet);
        }

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        var sixthPet = volunteer.Pets[5];

        // act
        var result = volunteer.MovePet(fifthPet, Position.Create(2).Value);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Should().Be(Position.Create(1).Value);
        secondPet.Position.Should().Be(Position.Create(3).Value);
        thirdPet.Position.Should().Be(Position.Create(4).Value);
        fourthPet.Position.Should().Be(Position.Create(5).Value);
        fifthPet.Position.Should().Be(Position.Create(2).Value);
        sixthPet.Position.Should().Be(Position.Create(6).Value);
    }
}