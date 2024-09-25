using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.DTO.Responses.Project
{
    public class GetAllProjectsResponse
    {
        public List<GetProjectResponse> Projects { get; set; }
    }
}
