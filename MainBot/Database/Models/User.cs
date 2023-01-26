using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBot.Database.Models;

[Table("users")]
public class User
{
      [Key]
      public Guid Uid { get; init; }
      public ulong DiscordId { get; init; }
      public DateTime JoinDate { get; init; }
      public string Name { get; set; }
      public string Discriminant { get; set; }
      public List<Event> Events { get; set; }
}
