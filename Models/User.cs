using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Role : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
}

public class User : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    // Optional: Link to Customer or Employee
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
