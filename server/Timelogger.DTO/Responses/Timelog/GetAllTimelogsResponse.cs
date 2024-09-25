using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.DTO.Responses.Timelog
{
    public class GetAllTimelogsResponse
    {
        public List<GetTimelogResponse> Timelogs { get; set; }
    }
}
