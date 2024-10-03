using System.ComponentModel.DataAnnotations;

namespace BotApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string ChatId { get; set; } = null!;
    }
}
