using System.ComponentModel.DataAnnotations;

public class Brand : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ImageUrl { get; set; }
} 