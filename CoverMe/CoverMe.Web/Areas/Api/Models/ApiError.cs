using System.Collections.Generic;

namespace CoverMe.Web.Areas.Api.Models;

public class ApiError
{
    public string Key { get; set; }
    public List<string> Errors { get; set; }
}