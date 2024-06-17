using MediatR;
using PianoMentor.Contract.Models.PianoMentor.Courses;

namespace PianoMentor.Contract.Courses;

public class GetCourseItemsWithFilterRequest(long userId, int courseItemTypeId) : IRequest<GetCourseItemsResponse>
{
    public long UserId { get; set; } = userId;
    public int CourseItemTypeId { get; set; } = courseItemTypeId;
}