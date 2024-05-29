using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.Contract.Models.PianoMentor.Exercises;
using PianoMentor.Contract.Models.PianoMentor.Quizzes;
using PianoMentor.DAL.Domain.PianoMentor.Courses;
using PianoMentor.DAL.Models.Identity;
using PianoMentor.DAL.Models.PianoMentor.Exercises;
using PianoMentor.DAL.Models.PianoMentor.Quizzes;
using PianoMentor.DAL.Models.PianoMentor.Texts;

namespace PianoMentor.DAL
{
	public class PianoMentorDbContext(
		DbContextOptions<PianoMentorDbContext> options) 
		: IdentityDbContext<PianoMentorUser, IdentityRole<long>, long>(options)
	{
		public DbSet<Domain.DataSet.DataSet> DataSets { get; set; }
		public DbSet<Domain.DataSet.BinaryData> Binaries { get; set; }
		public DbSet<Domain.DataSet.Storage> Storages { get; set; }
		public DbSet<Domain.DataSet.OneTimeLink> OneTimeLinks { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<CourseItem> CourseItems { get; set; }
		public DbSet<CourseItemType> CourseItemTypes { get; set; }
		public DbSet<CourseUserProgress> CourseUserProgresses { get; set; }
		public DbSet<CourseItemUserProgress> CourseItemUserProgresses { get; set; }
		public DbSet<CourseItemProgressType> CourseItemProgressTypes { get; set; }
		public DbSet<ViewPagerText> ViewPagerTexts { get; set; }
		public DbSet<ViewPagerTextNumberRanges> ViewPagerTextNumberRanges { get; set; }
		public DbSet<QuizQuestion> QuizQuestions { get; set; }
		public DbSet<QuizQuestionAnswer> QuizQuestionAnswers { get; set; }
		public DbSet<QuizQuestionUserAnswerLog> QuizQuestionUserAnswerLogs { get; set; }
		public DbSet<QuizQuestionType> QuizQuestionTypes { get; set; }
		public DbSet<ExerciseTask> ExerciseTasks { get; set; }
		public DbSet<ExerciseType> ExerciseTypes { get; set; }
		public DbSet<Interval> Intervals { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasDefaultSchema("PianoMentor");

			builder.Entity<PianoMentorUser>(e =>
			{
				e.Property(p => p.IsDeleted).HasDefaultValue(false);

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

				e.Property(p => p.Filename).IsUnicode(true).HasMaxLength(255).IsRequired();
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
					new { Id = 1, Name = "DevStorage", BasePath = @"C:\PianoMentor\DevStorage", AllowWrite = true }
				]);
			});

			builder.Entity<Domain.DataSet.OneTimeLink>(e =>
			{
				e.ToTable("OneTimeLinks");

				e.HasKey(p => p.UrlEncryptedToken);

				e.Property(p => p.LinkExpirationTime).IsRequired();
				e.Property(p => p.UrlEncryptedToken).IsUnicode(false).IsRequired();
			});

			builder.Entity<Course>(e =>
			{
				e.ToTable("Courses");

				e.HasKey(p => p.CourseId);

				e.Property(p => p.Title).IsRequired().HasMaxLength(100);
				e.Property(p => p.Subtitle).IsRequired().HasMaxLength(100);
				e.Property(p => p.Description).IsRequired().HasMaxLength(255);
				e.Property(p => p.UpdatedAt).IsRequired();
				e.Property(p => p.IsDeleted).HasDefaultValue(false);

				e.HasMany(p => p.CourseItems).WithOne(p => p.Course).OnDelete(DeleteBehavior.Cascade);
			});

			builder.Entity<CourseItem>(e => 
			{
				e.ToTable("CourseItems");

				e.HasKey(p => p.CourseItemId);

				e.Property(p => p.Title).IsRequired().HasMaxLength(100);
				e.Property(p => p.Position).IsRequired();
				e.Property(p => p.AttachedDataSetId).IsRequired(false);
				e.Property(p => p.UpdatedAt).IsRequired();
				e.Property(p => p.IsDeleted).HasDefaultValue(false);

				e.HasOne(p => p.AttachedDataSet).WithOne().HasForeignKey<CourseItem>(p => p.AttachedDataSetId).OnDelete(DeleteBehavior.NoAction);
				e.HasOne(p => p.Course).WithMany(p => p.CourseItems).HasForeignKey(p => p.CourseId).OnDelete(DeleteBehavior.Cascade);
				e.HasOne(p => p.CourseItemType).WithMany().HasForeignKey(p => p.CourseItemTypeId).OnDelete(DeleteBehavior.NoAction);
			});

			builder.Entity<CourseItemType>(e =>
			{
				e.ToTable("CourseItemTypes");

				e.HasKey(p => p.CourseItemTypeId);

				e.Property(p => p.Name).IsRequired().HasMaxLength(30);

				foreach (var v in Enum.GetValues<CourseItemTypesEnum>())
				{
					e.HasData(new { CourseItemTypeId = (int)v, Name = v.ToString() });
				}
			});

			builder.Entity<CourseUserProgress>(e =>
			{
				e.ToTable("CourseUsersProgresses");

				e.HasKey(p => p.Id);

				e.Property(p => p.ProgressInPercent);

				e.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
				e.HasOne(p => p.Course).WithMany().HasForeignKey(p => p.CourseId).OnDelete(DeleteBehavior.Cascade);
			});

			builder.Entity<CourseItemUserProgress>(e =>
			{
				e.ToTable("CourseItemsUsersProgresses");

				e.HasKey(p => p.Id);

				e.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
				e.HasOne(p => p.CourseItem).WithMany().HasForeignKey(p => p.CourseItemId).OnDelete(DeleteBehavior.Restrict);
				e.HasOne(p => p.CourseItemProgressType).WithMany().HasForeignKey(p => p.CourseItemProgressTypeId).OnDelete(DeleteBehavior.NoAction);
			});

			builder.Entity<CourseItemProgressType>(e =>
			{
				e.ToTable("CourseItemsProgressTypes");

				e.HasKey(p => p.Id);

				e.Property(p => p.Name).IsRequired().HasMaxLength(30);

				foreach (var v in Enum.GetValues<CourseItemProgressTypesEnum>())
				{
					e.HasData(new { Id = (int)v, Name = v.ToString() });
				}
			});

			builder.Entity<ViewPagerText>(e =>
			{
				e.ToTable("ViewPagerTexts");

				e.HasKey(p => p.Id);

				e.Property(p => p.IsDeleted).HasDefaultValue(false);

				e.HasMany(p => p.ViewPagerTextNumberRanges).WithOne().HasForeignKey(p => p.ViewPagerTextId).OnDelete(DeleteBehavior.Cascade);
			});

			builder.Entity<QuizQuestion>(e =>
			{
				e.ToTable("QuizQuestions");

				e.HasKey(p => p.QuestionId);

				e.Property(p => p.AttachedDataSetId).IsRequired(false);
				e.Property(p => p.IsDeleted).HasDefaultValue(false);
				e.Property(p => p.QuestionText).HasMaxLength(255);

				e.HasOne(p => p.AttachedDataSet).WithOne().HasForeignKey<QuizQuestion>(p => p.AttachedDataSetId).OnDelete(DeleteBehavior.NoAction);
				e.HasOne(p => p.CourseItem).WithMany().HasForeignKey(p => p.CourseItemId).OnDelete(DeleteBehavior.NoAction);
				e.HasOne(p => p.QuizQuestionType).WithMany().HasForeignKey(p => p.QuizQuestionTypeId).OnDelete(DeleteBehavior.NoAction);
			});

			builder.Entity<QuizQuestionAnswer>(e =>
			{
				e.ToTable("QuizQuestionsAnswers");

				e.HasKey(p => p.AnswerId);

				e.Property(p => p.AnswerText).HasMaxLength(255).IsRequired();
				e.Property(p => p.IsCorrect).IsRequired();
				e.Property(p => p.IsDeleted).HasDefaultValue(false);

				e.HasOne(p => p.QuizQuestions).WithMany(p => p.QuizQuestionsAnswers).HasForeignKey(p => p.QuizQuestionId).OnDelete(DeleteBehavior.Cascade);
			});

			builder.Entity<QuizQuestionType>(e =>
			{
				e.ToTable("QuizQuestionsTypes");

				e.HasKey(p => p.QuizQuestionTypeId);

				e.Property(p => p.Name).HasMaxLength(70).IsRequired();

				foreach (var v in Enum.GetValues<QuizQuestionTypeEnumeration>())
				{
					e.HasData(new { QuizQuestionTypeId = (int)v, Name = v.ToString() });
				}
			});

			builder.Entity<QuizQuestionUserAnswerLog>(e =>
			{
				e.ToTable("QuizQuestionsUsersAnswersLogs");

				e.HasKey(p => p.AnswerLogId);

				e.Property(p => p.UserAnswerText).IsRequired(false).HasMaxLength(150);

				e.HasOne(p => p.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);
				e.HasOne(p => p.Question).WithMany().HasForeignKey(e => e.QuestionId).OnDelete(DeleteBehavior.NoAction);
				e.HasOne(p => p.Answer).WithMany().HasForeignKey(e => e.AnswerId).OnDelete(DeleteBehavior.NoAction);
			});

			builder.Entity<ExerciseTask>(e =>
			{
				e.ToTable("ExerciseTasks");

				e.HasKey(p => p.ExerciseTaskId);

				e.Property(p => p.CourseItemId).IsRequired();
				e.Property(p => p.ExerciseTypeId).IsRequired();

				e.HasOne(p => p.CourseItem).WithMany().HasForeignKey(p => p.CourseItemId).OnDelete(DeleteBehavior.NoAction);
				e.HasOne(p => p.ExerciseType).WithMany().HasForeignKey(p => p.ExerciseTypeId).OnDelete(DeleteBehavior.NoAction);
				e.HasMany(p => p.IntervalsInTask).WithMany().UsingEntity(j => j.ToTable("IntervalsInTasks"));
			});

			builder.Entity<ExerciseType>(e =>
			{
				e.ToTable("ExerciseTypes");

				e.HasKey(p => p.ExerciseTypeId);

				e.Property(p => p.ExerciseTypeName).IsRequired().HasMaxLength(50);
				
				foreach (var v in Enum.GetValues<ExerciseTypesEnum>())
				{
					e.HasData(new { ExerciseTypeId = (int)v, ExerciseTypeName = v.ToString() });
				}
			});
			
			builder.Entity<Interval>(e =>
			{
				e.ToTable("Intervals");

				e.HasKey(p => p.IntervalId);

				e.Property(p => p.IntervalName).IsRequired().HasMaxLength(50);

				foreach (var v in Enum.GetValues<IntervalsEnum>())
				{
					e.HasData(new { IntervalId = (int)v, IntervalName = v.ToString() });
				}
			});

			base.OnModelCreating(builder);
		}

		public override int SaveChanges()
		{
			ApplyPercentLimit();
			return base.SaveChanges();
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			ApplyPercentLimit();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			ApplyPercentLimit();
			return base.SaveChangesAsync(cancellationToken);
		}

		private void ApplyPercentLimit()
		{
			foreach (var entry in ChangeTracker.Entries<CourseUserProgress>())
			{
				if (entry.State is not (EntityState.Added or EntityState.Modified))
				{
					continue;
				}
				
				var courseProgress = entry.Entity;
				if (courseProgress.ProgressInPercent > 100)
				{
					courseProgress.ProgressInPercent = 100;
				}
			}
		}
	}
}
