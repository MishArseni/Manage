﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Email")]
        public string Email { get; set; }
        public string Name { get; set; }

        [Required (ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
