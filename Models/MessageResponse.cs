namespace user_service.Models
{
    public class MessageResponse
    {
        public string Message { set; get; }
        public override string ToString()
        {
            return Util.FormatToJsonBody(this);
        }
    }
}
