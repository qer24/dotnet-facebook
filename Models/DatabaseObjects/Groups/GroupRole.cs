namespace dotnet_facebook.Models.DatabaseObjects.Groups
{
    [Flags]
    public enum GroupRole
    {
        Member = 1,
        Moderator = 2,
        Admin = 4
    }
}
