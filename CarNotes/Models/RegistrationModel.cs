using System.ComponentModel.DataAnnotations;

namespace CarNotes.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите свое имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [MinLength(6, ErrorMessage ="Пароль должен быть более 6-ти символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный email")]
        public string Email { get; set; }
    }
}