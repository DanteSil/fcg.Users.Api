namespace Fcg.Application.Users;

public sealed record UserCreatedEvent(Guid UserId, string Email, string Name);

public interface IUserCreatedPublisher
{
    Task PublishAsync(UserCreatedEvent userCreated);
}
