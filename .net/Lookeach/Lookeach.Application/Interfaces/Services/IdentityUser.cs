namespace Lookeach.Application.Interfaces.Services
{
    public interface IIdentityUser<T>
    {
        T UserId { get; }
    }
}
