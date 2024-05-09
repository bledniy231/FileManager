using MediatR;

namespace PianoMentor.Contract.Statistics
{
    public class GetUserStatisticsRequest(long userId) : IRequest<GetUserStatisticsResponse>
    {
        public long UserId { get; set; } = userId;
    }
}
