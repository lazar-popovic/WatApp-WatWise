using Microsoft.AspNetCore.Mvc;

namespace API.Models;

public class Response<T> : ActionResult
{
    public List<string> Errors { get; set; }
    public bool Success { get; set; }
    public List<T> Data { get; set; }

    public Response()
    {
        Errors = new List<string>();
    }
}