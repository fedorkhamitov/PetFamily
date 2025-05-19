using PetFamily.Domain.Entities;
using PetFamily.Domain.Share;

namespace PetFamily.Tests.TestInstances;

public class Instances
{
    public static Volunteer TestVolunteerInstance()
    {
        return new Volunteer(
            VolunteerId.NewId(),
            HumanName.Create("Test", "Test", "Test").Value,
            "test@test.ru",
            "test",
            3,
            "1234567890",
            null,
            null);
    }

    public static Pet TestPetInstance(string name = "Test")
    {
        return new Pet(
            name,
            "Test",
            false,
            false,
            new SpeciesInfo(Guid.NewGuid(), Guid.NewGuid()),
            "Test",
            Address.Create("111111", "Test", "Test", "Test", "Test", "1", "1").Value,
            3, 3,
            "1234567890",
            DateTime.Now,
            DonationDetails.Create("Test", "Test").Value,
            Position.Create(1).Value); //!!! I added it quickly so as not to worry - in a hurry...
    }
}