using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainBot.Database.Models;

[Table("event_to_users")]
public class EventToUser
{
    [Key]
    public Guid Uid { get; init; }
    
    public Guid EventUid { get; init; }
    [ForeignKey("EventUid")]
    public Event Event { get; init; }
    
    public Guid UserUid { get; init; }
    [ForeignKey("UserUid")]
    public User User { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Reason { get; init; }
    
}
