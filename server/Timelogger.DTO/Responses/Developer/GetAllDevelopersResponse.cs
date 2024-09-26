using System;
using System.Collections.Generic;
using System.Text;

namespace Timelogger.DTO.Responses.Developer
{
    public class GetAllDevelopersResponse
    {
        public List<GetDeveloperResponse> Developers {  get; set; }
    }
}
