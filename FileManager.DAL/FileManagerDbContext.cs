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
		public DbSet<Domain.PianoMentor.Course> Courses { get; set; }
		public DbSet<Domain.PianoMentor.CourseItem> CourseItems { get; set; }
		public DbSet<Domain.PianoMentor.CourseType> CourseTypes { get; set; }

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

			builder.Entity<Domain.PianoMentor.Course>(e =>
			{
				e.ToTable("Courses");

				e.HasKey(p => p.CourseId);

				e.Property(p => p.Title).IsRequired().HasMaxLength(30);
				e.Property(p => p.Subtitle).IsRequired().HasMaxLength(40);
				e.Property(p => p.Description).IsRequired().HasMaxLength(150);

				e.HasMany(p => p.CourseItems).WithOne(p => p.Course).OnDelete(DeleteBehavior.Cascade);
			});

			builder.Entity<Domain.PianoMentor.CourseItem>(e => 
			{
				e.ToTable("CourseItems");

				e.HasKey(p => p.CourseItemId);

				e.Property(p => p.Title).IsRequired().HasMaxLength(30);
				e.Property(p => p.Position).IsRequired();

				e.HasOne(p => p.Course).WithMany(p => p.CourseItems).HasForeignKey(p => p.CourseId).OnDelete(DeleteBehavior.Cascade);
				e.HasOne(p => p.CourseType).WithMany().HasForeignKey(p => p.CourseTypeId).OnDelete(DeleteBehavior.NoAction);
			});

			builder.Entity<Domain.PianoMentor.CourseType>(e =>
			{
				e.ToTable("CourseTypes");

				e.HasKey(p => p.CourseTypeId);

				e.Property(p => p.Name).IsRequired().HasMaxLength(30);
			});

			builder.Entity<Domain.PianoMentor.CourseType>().HasData(
			[
				new { CourseTypeId = 1, Name = "Lesson" },
				new { CourseTypeId = 2, Name = "Exercise" },
				new { CourseTypeId = 3, Name = "Quiz" }
			]);
			base.OnModelCreating(builder);
		}
	}
}
