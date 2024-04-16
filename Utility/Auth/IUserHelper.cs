namespace ITKANSys_api.Utility.Auth
{
    public interface IUserHelper
    {
        string GetCurrentUserId();
        string GetCurrentUserName();
        string GetCurrentUserRoleId();
    }
}
