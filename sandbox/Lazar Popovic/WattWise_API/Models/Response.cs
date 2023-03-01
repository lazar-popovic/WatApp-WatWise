using Microsoft.AspNetCore.Mvc;

namespace WattWise_API.Models
{
    public class Response : ActionResult
    {
        public List<string> Error { get; set; }
        public bool Success { get; set; }
        public BaseViewModel Data { get; set; }

        public Response()
        {
            Error = new List<string>();
        }

        
    }
}
