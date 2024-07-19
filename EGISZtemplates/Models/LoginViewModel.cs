using System.ComponentModel.DataAnnotations;

namespace EGISZtemplates.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Вы пытаетесь войти в аккаунт без эмейла? Ха! НЕ ВЫЙДЕТ!")]
        [EmailAddress(ErrorMessage = "Это не похоже на действительный адрес электронной почты.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ТАК ПРОСТО ВАМ НЕ ПОПАСТЬ В СИСТЕМУ, СПЕРВА - НУЖЕН ПАРОЛЬ!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
