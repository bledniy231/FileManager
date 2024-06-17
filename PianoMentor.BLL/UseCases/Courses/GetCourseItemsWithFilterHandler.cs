using MediatR;
using Microsoft.EntityFrameworkCore;
using PianoMentor.Contract.Courses;
using PianoMentor.Contract.Models.PianoMentor.Courses;
using PianoMentor.DAL;

namespace PianoMentor.BLL.UseCases.Courses;

internal class GetCourseItemsWithFilterHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetCourseItemsWithFilterRequest, GetCourseItemsResponse>
{
    public Task<GetCourseItemsResponse> Handle(GetCourseItemsWithFilterRequest request, CancellationToken cancellationToken)
    {
        try
        {
        	var courseItemsUserProgressesNames = dbContext.CourseItemUserProgresses
        		.AsNoTracking()
        		.Where(ciup =>
        			ciup.UserId == request.UserId
        			&& ciup.CourseItem.CourseItemTypeId == request.CourseItemTypeId)
        		.Select(ciup => new
        		{
        			ciup.CourseItemId,
        			ciup.CreatedAt,
        			ProgressName = ciup.CourseItemProgressType.Name
        		})
        		.ToList();

        	var courseItems = dbContext.CourseItems
        		.AsNoTracking()
        		.Where(ci => ci.CourseItemTypeId == request.CourseItemTypeId)
        		.Select(ci => new
        		{
        			ci.CourseItemId,
        			ci.Position,
        			ci.Title,
        			ci.CourseId,
        			ci.CourseItemType
        		})
        		.AsEnumerable()
        		.Select(ci => new CourseItemModel
        		{
        			CourseItemId = ci.CourseItemId,
        			Position = ci.Position,
        			Title = ci.Title,
        			CourseId = ci.CourseId,
        			CourseItemType = ci.CourseItemType.Name,
        			CourseItemProgressType = courseItemsUserProgressesNames
        					.Where(ciupn => ciupn.CourseItemId == ci.CourseItemId)
        					.OrderByDescending(ciupn => ciupn.CreatedAt)
        					.Select(ciupn => ciupn.ProgressName)
        					.DefaultIfEmpty("NotStarted")
        					.FirstOrDefault()!
        		})
        		.ToList();


        	return Task.FromResult(new GetCourseItemsResponse(courseItems, null));
        }
        catch (Exception ex)
        {
        	return Task.FromResult(new GetCourseItemsResponse(null, [ex.Message, ex.StackTrace]));
        }
    }
}