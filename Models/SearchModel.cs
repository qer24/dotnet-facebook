using dotnet_facebook.Models.DatabaseObjects.Groups;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using dotnet_facebook.Models.DatabaseObjects.Users;
using System.Collections;

namespace dotnet_facebook.Models;

public class SearchModel
{
    public string Query { get; set; }

    public List<MainPost> Posts = [];
    public List<User> Users = [];
    public List<Group> Groups = [];
}
