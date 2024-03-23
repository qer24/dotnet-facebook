using dotnet_facebook.Models.DatabaseObjects.Users;

namespace dotnet_facebook.Models.DatabaseObjects
{
    public class GroupUser
    {
        public bool IsModerator { get; set; }
        public virtual User Users { get; set; }
        public virtual Group Groups { get; set; }

    }
}
