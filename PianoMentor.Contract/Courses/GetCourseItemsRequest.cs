using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoMentor.Contract.Courses
{
	public class GetCourseItemsRequest : IRequest<GetCourseItemsResponse>
	{
		public long UserId { get; set; } = 0;
		public int CourseId { get; set; } = 0;
	}
}
