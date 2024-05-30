namespace SkudWebApplication.Dto
{
    public class LoginResponse
    {
        public bool Flag { get; set; }
        public string Message { get; set; } = null!;
        public string JWTToken { get; set;} = null!;
    }
}
