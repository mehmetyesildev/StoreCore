using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Data.Concrete;
using StoreApp.Web.Models;

namespace StoreApp.Web.Controllers;

public class AccountController:Controller
{
  private readonly UserManager<AppUser> _userManager;
  private readonly SignInManager<AppUser> _signInManager;
  private readonly RoleManager<AppRole> _roleManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
  {
    _userManager=userManager;
    _signInManager=signInManager;
    _roleManager=roleManager;
  }
    public IActionResult Register()
  {
     return View("UyeOl");
  }  
  [HttpPost]
  [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(ModelState.IsValid)
        {
            var user= new AppUser
            {
                UserName=model.UserName,
                FirstName=model.FirstName,
                LastName=model.LastName,
                Email=model.Email,
                Address=model.Address,
                City=model.City
            };
            IdentityResult result=await _userManager.CreateAsync(user,model.Password!);
            if (result.Succeeded)
            {
                // var token=await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // var url=Url.Action("C")
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index","Home");
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("",err.Description);
            }
            
        }
        return View(model);
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>Login(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
            var user=await _userManager.FindByEmailAsync(model.Email!);
            if(user!=null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password!, model.RememberMe, true);
                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEnabledAsync(user,true);
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeleft = lockoutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesabınız kitlendi, Lütfen {timeleft.Minutes}dakika sonra deneyiniz");
                }
                else
                {
                    ModelState.AddModelError("", "Parolanız hatalı");
                }
            }
            else
            {
                ModelState.AddModelError("", "Bu email adresi ile hesap bulunamadı");
            }

        }
        return View();
       
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

}
