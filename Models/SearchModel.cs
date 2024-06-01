using dotnet_facebook.Models.DatabaseObjects.Groups;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using System.Collections;

namespace dotnet_facebook.Models;

public class SearchModel
{
    public string Query { get; set; }

    public List<MainPost> Posts { get; set; }
    public List<User> Users { get; set; }
    public List<Group> Groups { get; set; }
}
