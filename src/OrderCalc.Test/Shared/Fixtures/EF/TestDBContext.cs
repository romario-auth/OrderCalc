using Microsoft.EntityFrameworkCore;
using OrderCalc.Infrastructure.Context;

namespace OrderCalc.Test.Shared.Fixtures.EF;

public static class TestDBContext
{
    public static AppDbContext GetInMemoryDbContext(string dbName = "TestDb")
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new AppDbContext(options);

        context.Database.EnsureCreated();
        return context;
    }
}

