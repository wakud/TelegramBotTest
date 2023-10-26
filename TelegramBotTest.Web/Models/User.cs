using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBotTest.Web.Models
{
    [Table("AppUser")]
    public class User
    {
        [Key] public int Id { get; set; }
        [Required] public long ChatId { get; set; }
        [Required] public string? UserName { get; set; }
        [Required] public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public DateTime CreatedUser { get; set; } = DateTime.Now;
    }
}
