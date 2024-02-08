using API.Common;

namespace API.Entities
{
    public class CountrySearchLog : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid CountryId { get; set; }

    }
}