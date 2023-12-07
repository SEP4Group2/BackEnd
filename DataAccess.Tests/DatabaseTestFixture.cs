using Microsoft.EntityFrameworkCore;
using AppContext = DataAccess.AppContext;

namespace Tests.DataAccess;

[TestFixture]
public class DatabaseTestFixture
{
    public DbContextOptions<AppContext> Options { get; private set; }
    public AppContext Context { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Options = new DbContextOptionsBuilder<AppContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        Context = new AppContext(Options);
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
