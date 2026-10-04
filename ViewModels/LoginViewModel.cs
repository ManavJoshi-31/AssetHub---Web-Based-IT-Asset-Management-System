using System.ComponentModel.DataAnnotations;

namespace AssetHub.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    // Option to remain signed In across multiple browser sessions
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}