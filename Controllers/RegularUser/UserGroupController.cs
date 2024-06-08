using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using dotnet_facebook.Models.DatabaseObjects.Posts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotnet_facebook.Controllers.RegularUser;

[Route("UserGroup")]
public class UserGroupController(TestContext context, GroupService groupService, UserService userService, TagsService tagsService, PostService postService) : Controller
{
    // GET: UserGroup
    [HttpGet("{id?}")]
    [OutputCache(NoStore = true, Duration = 0)]
    public async Task<IActionResult> Index(int? id)
        {
        if (id == null)
        {
            return RedirectToAction("GroupNotFound");
        }

        var group = await groupService.GetGroupByIdAsync(id);
        if (group == null)
        {
            return RedirectToAction("GroupNotFound");
        }
        var userRole = group.Users.FirstOrDefault(u => u.User.Nickname == User.Identity.Name)?.GroupRole;

        tagsService.GenerateTagsBag(ViewBag);
        userService.GenerateLocalUserBag(ViewBag, User);

        ViewBag.IsAdmin = userRole == GroupRole.Admin;
        ViewBag.IsMember = userRole == GroupRole.Member || userRole == GroupRole.Moderator || userRole == GroupRole.Admin;
        ViewBag.IsModerator = userRole == GroupRole.Moderator || userRole == GroupRole.Admin;
        ViewBag.GroupTags = group.Tags;
        ViewBag.Roles = Enum.GetValues(typeof(GroupRole)).Cast<GroupRole>().Select(r => new SelectListItem
        {
            Value = r.ToString(),
            Text = r.ToString()
        }).ToList();

        ViewBag.GroupPosts = await context.MainPosts
            .Where(p => p.ParentGroup == group)
            .Include(p => p.OwnerUser)
            .ThenInclude(u => u.UserProfile)
            .Include(p => p.Tags)
            .Include(p => p.Likes)
            .ToListAsync();

        return View(group);
    }

    [HttpPost("Manage")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Manage(int? GroupId, int? UserId)
    {
        if (GroupId == null)
        {
            return NotFound();
        }

        var @group = await context.Groups
            .Include(g => g.Users)
            .ThenInclude(gu => gu.User)
            .FirstOrDefaultAsync(m => m.GroupId == GroupId);
        if (@group == null)
        {
            return NotFound();
        }

        var selectedUserRoleString = Request.Form["selectedUserRole"];

        var groupUser = @group.Users.FirstOrDefault(gu => gu.User.UserId == UserId);

        if (groupUser == null)
        {
            return Json(new { success = false, error = "User not found in group." });
        }
        // make sure user is not the last admin in the group
        else if (groupUser.GroupRole == GroupRole.Admin && @group.Users.Count(gu => gu.GroupRole == GroupRole.Admin) == 1)
        {
            return Json(new { success = false, error = "Cannot remove the last admin from the group." });
        }
        else
        {
            var selectedUserRole = Enum.Parse<GroupRole>(selectedUserRoleString);
            groupUser.GroupRole = selectedUserRole;

            context.Update(@group);
            await context.SaveChangesAsync();
        }

        GenerateUsersBag();
        GenerateRolesBag();

        return Json(new { success = true });
    }

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser(int? GroupId)
    {
        var localUser = await userService.GetLocalUserAsync(User);

        if (GroupId == null)
        {
            return RedirectToAction("GroupNotFound");
        }

        var @group = await context.Groups
            .Include(g => g.Users)
            .ThenInclude(gu => gu.User)
            .FirstOrDefaultAsync(m => m.GroupId == GroupId);
        if (@group == null)
        {
            return RedirectToAction("GroupNotFound");
        }

        var userStringError = "";

        // if user is already in the group, throw error
        var selectedUserId= localUser.UserId;
        if (selectedUserId == null)
        {
            userStringError = "Please select a user.";
        }
        else
        {
            var userAlreadyInGroup = @group.Users.Any(gu => gu.User.UserId == selectedUserId);

            if (userAlreadyInGroup)
            {
                userStringError = "User is already in the group.";
            }
            else
            {
                var user = context.Users.Find(selectedUserId);
                @group.Users.Add(new GroupUser
                {
                    User = user,
                    Group = @group,
                    GroupRole = GroupRole.Member
                });

                try
                {
                    context.Update(@group);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupId))
                    {
                        return RedirectToAction("GroupNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        GenerateUsersBag();
        GenerateRolesBag();

        return RedirectToAction("Index", new { id = @group.GroupId });
    }

    private bool GroupExists(int id)
    {
        return context.Groups.Any(e => e.GroupId == id);
    }



    private void GenerateUsersBag()
    {
        ViewBag.Users = context.Users.Select(u => new SelectListItem
        {
            Value = u.UserId.ToString(),
            Text = u.Nickname
        }).ToList();
    }

    private void GenerateRolesBag()
    {
        ViewBag.Roles = Enum.GetValues(typeof(GroupRole)).Cast<GroupRole>().Select(r => new SelectListItem
        {
            Value = r.ToString(),
            Text = r.ToString()
        }).ToList();
    }

    [HttpPost("UpdateBio")]
    public async Task<IActionResult> UpdateBio(int GroupId, string GroupDescription)
    {
        var group = await groupService.GetGroupByIdAsync(GroupId);
        if (group == null)
        {
            return RedirectToAction("GroupNotFound");
        }

        // if the group bio is null, set it to an empty string
        GroupDescription ??= "";

        group.GroupDescription = GroupDescription;

        await context.SaveChangesAsync();

        return RedirectToAction("Index", new { id = GroupId });
    }
    [HttpPost("UpdateGroupPicture")]
    [EnableCors("AllowAllOrigins")]
    public async Task<IActionResult> UpdateGroupPicture(IFormFile file, int GroupId)
    {
        if (file != null && file.Length > 0)
        {
            var group = await groupService.GetGroupByIdAsync(GroupId);

            if (group == null)
            {
                return Json(new { success = false, message = "Group not found" });
            }

            var fileExtension = Path.GetExtension(file.FileName);
            // file name is the user id + the file extension
            var fileName = $"grouppfp_{group.GroupId}";

            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploadedFiles", fileName + fileExtension);

            // first, delete any existing file with the same name (any extension)
            var existingFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/uploadedFiles"), $"{fileName}.*");
            foreach (var existingFile in existingFiles)
            {
                System.IO.File.Delete(existingFile);
            }

            using var stream = new FileStream(path, FileMode.Create);

            await file.CopyToAsync(stream);

            group.GroupPictureFileName = fileName + fileExtension;

            await context.SaveChangesAsync();

            return Json(new { success = true, message = "File uploaded successfully" });
        }

        return Json(new { success = false, message = "No file uploaded" });
    }
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(int GroupId)
    {
        var @group = await context.Groups.FindAsync(GroupId);
        if (@group != null)
        {
            context.Groups.Remove(@group);
        }

        await context.SaveChangesAsync();
        return RedirectToAction("GroupList");
    }
    [HttpPost("RemoveUser")]
    public async Task<IActionResult> RemoveUser(int? userId, int? groupId)
    {
        if (userId == null || groupId == null)
        {
            return NotFound();
        }

        var @group = await context.Groups
            .Include(g => g.Users)
            .ThenInclude(gu => gu.User)
            .FirstOrDefaultAsync(m => m.GroupId == groupId);
        if (@group == null)
        {
            return NotFound();
        }

        var @user = await context.Users.FindAsync(userId);
        if (@user == null)
        {
            return NotFound();
        }

        // Can't remove last admin
        if (@group.Users.Count(gu => gu.GroupRole == GroupRole.Admin) == 1 && @group.Users.Any(gu => gu.User.UserId == userId && gu.GroupRole == GroupRole.Admin))
        {
            return RedirectToAction("Index", new { id = @group.GroupId });
        }

        var groupUser = @group.Users.FirstOrDefault(gu => gu.User.UserId == userId);
        if (groupUser != null)
        {
            @group.Users.Remove(groupUser);

            context.Update(@group);
            await context.SaveChangesAsync();
        }

        return RedirectToAction("Index", new { id = @group.GroupId });
    }
    [HttpGet("GroupList")]
    public async Task<IActionResult> GroupList()
    {
        tagsService.GenerateTagsBag(ViewBag);
        ViewBag.localUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var groups = await userService.GetGroupsForLocalUserAsync(User);
        return View(groups); // Or return Json(groups) if you prefer a JSON response
    }
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("GroupName,GroupDescription,Tags")] Models.DatabaseObjects.Groups.Group group, string selectedUserId, string[] selectedTagIds)
    {
        if (string.IsNullOrWhiteSpace(selectedUserId))
        {
            ModelState.AddModelError("selectedUserId", "Please select a user.");
        }
        else
        {
            var selectedUserIdInt = Convert.ToInt32(selectedUserId);
            var user = await context.Users.FindAsync(selectedUserIdInt);
            group.Users = new List<GroupUser>
            {
                new GroupUser
                {
                    User = user,
                    GroupRole = GroupRole.Admin
                }
            };
        }

        group.GroupCreationDate = DateTime.Now;

        if (context.Groups.Any(g => g.GroupName == group.GroupName))
        {
            ModelState.AddModelError("GroupName", "Group Name already exists!");
        }

        // Retrieve selected tags from their IDs
        var selectedTags = await context.Tags.Where(t => selectedTagIds.Contains(t.TagId.ToString())).ToListAsync();

        // Assign selected tags to the group
        group.Tags = selectedTags;

        if (ModelState.IsValid)
        {
            context.Add(group);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = group.GroupId });
        }

        // Repopulate ViewBag.Users to maintain the data on validation failure
        GenerateUsersBag();

        // Populate ViewBag.Tags to maintain the data on validation failure
        ViewBag.Tags = await context.Tags.Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName }).ToListAsync();

        return View(group);
    }

    [HttpGet("GroupNotFound")]
    public IActionResult GroupNotFound()
    {
        return View();
    }

    [HttpPost("NewPost")]
    public async Task<IActionResult> NewPost([Bind("PostId,Content")] MainPost mainPost, List<string> selectedTagIds, int GroupId)
    {
        var selectedTagsIdsInt = selectedTagIds.Select(int.Parse).ToArray();
        var tags = tagsService.GetTagsByIds(selectedTagsIdsInt);

        mainPost.Tags = tags.ToArray();

        mainPost.ParentGroup = await groupService.GetGroupByIdAsync(GroupId);

        await postService.Create(mainPost, User, ModelState, Request.Cookies);

        return RedirectToAction(nameof(Index), new { id = GroupId } );
    }

    [HttpPost]
    public async Task<IActionResult> LikePost(int postId)
    {
        var likeResult = await Like(postId);

        switch (likeResult)
        {
            case LikeResult.AlreadyLiked:
                return Json(new { success = true, alreadyLiked = true });
            case LikeResult.Success:
                return Json(new { success = true });
            default:
                return Json(new { success = false, message = "An error occurred while liking the post." });
        }
    }

    private enum LikeResult { Success, Error, AlreadyLiked }

    private async Task<LikeResult> Like(int? postId)
    {
        if (postId == null)
        {
            return LikeResult.Error;
        }

        Post? post = await context.MainPosts
            .Include(p => p.Likes)
            .ThenInclude(l => l.User)
            .FirstOrDefaultAsync(p => p.PostId == postId);

        if (post == null || post.Likes == null)
        {
            post = await context.Comments
                .Include(p => p.Likes)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        if (post == null || post.Likes == null)
        {
            return LikeResult.Error;
        }

        // get the user id from the identity
        var localUser = await userService.GetLocalUserAsync(User);

        Console.WriteLine(localUser);

        if (localUser == null)
        {
            return LikeResult.Error;
        }

        var previousLike = post.Likes.FirstOrDefault(l => l.User.UserId == localUser.UserId);

        if (previousLike != null)
        {
            post.Likes.Remove(previousLike);
            await context.SaveChangesAsync();
            return LikeResult.AlreadyLiked;
        }

        post.Likes.Add(new Like
        {
            Post = post,
            User = localUser,
            LikeDate = DateTime.Now
        });
        await context.SaveChangesAsync();

        return LikeResult.Success;
    }
} 
