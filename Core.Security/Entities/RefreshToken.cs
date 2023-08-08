using Core.Persistence.Repositories;

namespace Core.Security.Entities
{
    public class RefreshToken : Entity
    {


        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime ExpireDate { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? RevokedReason { get; set; }

        public virtual User User { get; set; }

        public RefreshToken()
        {

        }

        public RefreshToken(int userId, string token, DateTime created, DateTime expireDate, string createdByIp, DateTime revoked, string? revokedByIp, string? replacedByToken, string? revokedReason)
        {
            UserId = userId;
            Token = token;
            Created = created;
            ExpireDate = expireDate;
            CreatedByIp = createdByIp;
            Revoked = revoked;
            RevokedByIp = revokedByIp;
            ReplacedByToken = replacedByToken;
            RevokedReason = revokedReason;
        }




    }
}