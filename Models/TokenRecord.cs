namespace nxtool.Models
{
    public class TokenRecord
    {
        public int Id { get; set; }
        public string HashedToken { get; set; }
        public string TokenHash { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}
