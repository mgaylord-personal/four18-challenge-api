
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Four18.Challenge.Data.Entities;

[Table("customer")]
public class Customer {
    [Key]
    [Column("customer_id")]
    public int Id { get; set; }

    [Required]
    [Column("first")]
    public string First { get; set; } = default!;

    [Required]
    [Column("last")]
    public string Last { get; set; } = default!;

    [Required]
    [Column("email")] 
    public string? Email { get; set; }

    [Required] 
    [Column("created_at")] 
    public DateTimeOffset CreatedAt { get; set; }

    [Column("modified_at")]
    public DateTimeOffset ModifiedAt { get; set; }
}
