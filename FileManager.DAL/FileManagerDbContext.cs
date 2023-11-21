using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FileManager.DAL.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FileManager.DAL
{
	public class FileManagerDbContext(
		DbContextOptions<FileManagerDbContext> options) 
		: IdentityDbContext<FileManagerUser, IdentityRole<long>, long>(options)
	{
		public DbSet<Domain.DataSet.DataSet> DataSets { get; set; }
		public DbSet<Domain.DataSet.BinaryData> Binaries { get; set; }
		public DbSet<Domain.DataSet.Storage> Storages { get; set; }
		public DbSet<Domain.DataSet.OneTimeLink> OneTimeLinks { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasDefaultSchema("FileManager");

			builder.Entity<FileManagerUser>(e =>
			{
				e.HasMany(p => p.DataSets).WithOne(p => p.Owner).OnDelete(DeleteBehavior.NoAction);
			});

			builder.Entity<IdentityRole<long>>().HasData(
			[
				new() { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
				new() { Id = 2, Name = "User", NormalizedName = "USER"}
			]);

			builder.Entity<Domain.DataSet.DataSet>(e =>
			{
				e.ToTable("DataSets");

				e.Property(p => p.Id).UseIdentityColumn(1, 1);

				e.HasMany(p => p.Binaries).WithOne(p => p.DataSet).OnDelete(DeleteBehavior.Cascade);

				e.HasOne(p => p.Storage);
			});

			builder.Entity<Domain.DataSet.BinaryData>(e =>
			{
				e.ToTable("Binaries");

				e.HasKey(p => new { p.DataSetId, p.DataId }).IsClustered();

				e.Property(p => p.Filename).IsUnicode(false).HasMaxLength(255).IsRequired();
			});

			builder.Entity<Domain.DataSet.Storage>(e =>
			{
				e.ToTable("Storages");

				e.Property(p => p.Id).UseIdentityColumn(1, 1);
				e.HasKey(p => p.Id).IsClustered();

				e.Property(p => p.Name).IsRequired();
				e.Property(p => p.BasePath).HasMaxLength(255).IsRequired();

				e.HasData(
				[
					new { Id = 1, Name = "DevStorage", BasePath = "C:\\FileManager\\DevStorage", AllowWrite = true }
				]);
			});

			builder.Entity<Domain.DataSet.OneTimeLink>(e =>
			{
				e.ToTable("OneTimeLinks");

				e.HasKey(p => p.UrlEncryptedToken);

				e.Property(p => p.LinkExpirationTime).IsRequired();
				e.Property(p => p.UrlEncryptedToken).IsUnicode(false).IsRequired();
			});

			base.OnModelCreating(builder);
		}
	}
}
