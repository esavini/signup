using System.ComponentModel.DataAnnotations;

namespace signup.Models;

public class Register {
    [Required]
    public string? Name { get; set; }
    
    [Required]
    public string? Surname { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords don't match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public Role Role { get; set; }
}

public enum Gender {
    Male = 0,
    Female = 1,
    Null = 2
}

public enum Role {
    Student = 0,
    Teacher = 1,
    Parent = 2,
    None = 3
}