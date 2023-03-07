using Microsoft.AspNetCore.Mvc;

namespace API.Models;

public class Response
{
    public List<string> Errors { get; set; }
    public bool Success { get; set; }
    public object Data { get; set; }

    public Response()
    {
        Errors = new List<string>();
    }
}