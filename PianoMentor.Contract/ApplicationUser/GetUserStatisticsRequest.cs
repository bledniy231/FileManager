using MediatR;

namespace PianoMentor.Contract.ApplicationUser
{
	public class GetUserStatisticsRequest(long userId) : IRequest<GetUserStatisticsResponse>
	{
		public long UserId { get; set; } = userId;
	}
}
