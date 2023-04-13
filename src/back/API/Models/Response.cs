using Microsoft.AspNetCore.Mvc;

namespace API.Models;

public class Response<T>
{
    public List<string> Errors { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; }

    public Response()
    {
        Errors = new List<string>();
    }
}