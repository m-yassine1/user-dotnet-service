namespace user_service.Models
{
    public class LoginRequest
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public override string ToString()
        {
            return Util.FormatToJsonBody(this);
        }
    }
}
