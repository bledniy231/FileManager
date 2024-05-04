using MediatR;

namespace PianoMentor.Contract.ApplicationUser
{
	public class GetCoursesUserStatisticsRequest(long userId) : IRequest<GetCoursesUserStatisticsResponse>
	{
		public long UserId { get; set; } = userId;
	}
}
