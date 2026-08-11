using GymMangamentSystem.Core.IServices;
using GymMangamentSystem.Reposatory.Services;
using Microsoft.EntityFrameworkCore;

namespace GymMangamentSystem.IntegrationTests.Services;

public class SoftDeleteInterceptorIntegrationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Remove_OnSoftDeletableEntity_HidesRowWithoutDeletingIt(bool useAsyncSave)
    {
        var databaseName = $"GymMangamentSystem_SoftDeleteTests_{Guid.NewGuid():N}";
        const string expectedPrefix = "GymMangamentSystem_SoftDeleteTests_";
        if (!databaseName.StartsWith(expectedPrefix, StringComparison.Ordinal))
            throw new InvalidOperationException("Unsafe integration test database name.");

        var connectionString =
            $"Server=(localdb)\\MSSQLLocalDB;Database={databaseName};" +
            "Trusted_Connection=True;TrustServerCertificate=True;";
        var options = new DbContextOptionsBuilder<SoftDeleteTestContext>()
            .UseSqlServer(connectionString)
            .AddInterceptors(new SoftDeleteInterceptor())
            .Options;

        await using var context = new SoftDeleteTestContext(options);
        try
        {
            await context.Database.EnsureCreatedAsync();
            var entity = new SoftDeleteTestEntity { Name = "keep row" };
            context.Entities.Add(entity);
            await SaveChanges(context, useAsyncSave);

            context.Entities.Remove(entity);
            var beforeDelete = DateTime.UtcNow;
            await SaveChanges(context, useAsyncSave);
            context.ChangeTracker.Clear();

            Assert.Empty(await context.Entities.ToListAsync());

            var stored = await context.Entities.IgnoreQueryFilters().SingleAsync();
            Assert.True(stored.IsDeleted);
            Assert.NotNull(stored.DeletedAt);
            Assert.True(stored.DeletedAt >= beforeDelete);
        }
        finally
        {
            await context.Database.EnsureDeletedAsync();
        }
    }

    private static async Task SaveChanges(SoftDeleteTestContext context, bool useAsyncSave)
    {
        if (useAsyncSave)
            await context.SaveChangesAsync();
        else
            context.SaveChanges();
    }

    private sealed class SoftDeleteTestContext(DbContextOptions<SoftDeleteTestContext> options) : DbContext(options)
    {
        public DbSet<SoftDeleteTestEntity> Entities => Set<SoftDeleteTestEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SoftDeleteTestEntity>().HasQueryFilter(entity => !entity.IsDeleted);
        }
    }

    private sealed class SoftDeleteTestEntity : ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
