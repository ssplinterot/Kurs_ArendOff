using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_ArendOff.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public string Login { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty; // Храним хэш пароля

        public string FullName { get; set; } = "Пользователь";
        public string Role { get; set; } = "User"; // User / Admin
    }
}
