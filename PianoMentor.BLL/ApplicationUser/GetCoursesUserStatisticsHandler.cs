using PianoMentor.Contract.ApplicationUser;
using PianoMentor.DAL;
using MediatR;

namespace PianoMentor.BLL.ApplicationUser
{
	internal class GetCoursesUserStatisticsHandler(PianoMentorDbContext dbContext) : IRequestHandler<GetCoursesUserStatisticsRequest, GetCoursesUserStatisticsResponse>
	{
		public Task<GetCoursesUserStatisticsResponse> Handle(GetCoursesUserStatisticsRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
