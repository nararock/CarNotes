using System.ComponentModel.DataAnnotations;


namespace CarNotes.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный email")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть более 6-ти символов")]
        public string Password { get; set; }
    }
}