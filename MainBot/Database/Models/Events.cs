using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBot.Database.Models;

[Table("events")]
public class Event
{
    [Key]
    public Guid Uid { get; init; }
    public string Name { get; set; }
    public List<User> Users { get; set; }
}
