using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RegistrationModel
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [MinLength(6, ErrorMessage ="Пароль должен быть более 6-ти символов")]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}