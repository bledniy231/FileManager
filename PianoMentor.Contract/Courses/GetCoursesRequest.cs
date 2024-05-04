using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoMentor.Contract.Courses
{
	public class GetCoursesRequest : IRequest<GetCoursesResponse>
	{
		public long UserId { get; set; } = 0;
	}
}
