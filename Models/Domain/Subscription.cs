namespace CsApi.Models.Domain;

public class Subscription
{
    public int Id { get; set; }
    public int SubscriberId { get; set; }
    public int SubscribedUserId { get; set; }

    public User Subscriber { get; set; }
    public User SubscribedUser { get; set; }
}
