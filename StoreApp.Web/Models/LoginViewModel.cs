using System.ComponentModel.DataAnnotations;

namespace StoreApp.Web.Models;

public class LoginViewModel
{

    [Required(ErrorMessage = "E-mail zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool RememberMe { get; set; } = true;
    public string? ReturnUrl { get; set; }
}
