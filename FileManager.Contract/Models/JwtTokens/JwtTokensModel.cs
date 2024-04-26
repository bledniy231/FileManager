namespace FileManager.Contract.Models.JwtTokens
{
    public class JwtTokensModel
    {
        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpireTime { get; set; } = null;
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; } = null;
    }
}
