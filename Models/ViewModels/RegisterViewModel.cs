using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Введите Email!")]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите имя!")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }

    }
}
