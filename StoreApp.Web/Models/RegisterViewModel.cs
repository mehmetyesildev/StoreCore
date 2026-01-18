using System.ComponentModel.DataAnnotations;

namespace StoreApp.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Soyad alanı zorunludur.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "E-mail zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
    public string? City { get; set; } 
    public string? Address { get; set; }
}
