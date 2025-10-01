using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERestaurant.Domain.Entity
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public int TenantId { get; set; }

        [Required, MaxLength(50)]
        public string CreatedBy { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
