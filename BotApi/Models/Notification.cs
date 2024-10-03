using System.ComponentModel.DataAnnotations;

namespace BotApi.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; } = null!;
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
