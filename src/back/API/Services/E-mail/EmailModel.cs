namespace API.Services.E_mail
{
    public class EmailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ToEmail { get; set; }

        public EmailModel(string subject, string body, string toEmail)
        {
            Subject = subject;
            Body = body;
            ToEmail = toEmail;
        }
    }
}
