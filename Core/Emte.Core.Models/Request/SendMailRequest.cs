namespace Emte.Core.Models.Request
{
    public class SendMailRequest
    {
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
        public string? Body { get; set; }
        public string? Subject { get; set; }
    }
}