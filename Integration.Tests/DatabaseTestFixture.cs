using Microsoft.EntityFrameworkCore;
using NUnit.Framework;


[TestFixture]
public class DatabaseTestFixture
{
    public DbContextOptions<DataAccess.AppContext> Options { get; private set; }
    public DataAccess.AppContext Context { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Options = new DbContextOptionsBuilder<DataAccess.AppContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        Context = new DataAccess.AppContext(Options);

    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Context.Dispose();
    }

    public async void ClearDatabase()
    {
        Context.Devices.RemoveRange(Context.Devices);
        Context.Users.RemoveRange(Context.Users);
        Context.Presets.RemoveRange(Context.Presets);
        Context.Plants.RemoveRange(Context.Plants);
        Context.PlantData.RemoveRange(Context.PlantData);            
        await Context.SaveChangesAsync();

    }
}