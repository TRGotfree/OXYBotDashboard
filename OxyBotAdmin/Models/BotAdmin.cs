using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class BotAdmin
    {
        [Range(1, 1000000)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите логин!")]
        [MaxLength(100, ErrorMessage = "В данном логине слишком много символов!")]
        [MinLength(3, ErrorMessage = "Слишком короткий логин!")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Укажите пароль!")]
        [MaxLength(64, ErrorMessage = "В данном пароле слишком много символов!")]
        [MinLength(5, ErrorMessage = "Слишком короткий пароль!")]
        public string Password { get; set; }

        public string Name { get; set; }

        public bool State { get; set; }

        public string Role { get; } = "admin";
    }
}
