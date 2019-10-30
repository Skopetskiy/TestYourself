using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Data;
using TestYourself.Domain;

namespace TestYourself.Services
{
  public class PostService : IPostService
  {
    private readonly DataContext _context;

    public PostService(DataContext context)
    {
      _context = context;
    }

    public async Task<bool> DeletePost(Guid postId)
    {
      var post = await GetPostById(postId);

      if (post != null)
      {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
      }

      else return false;
    }

    public async Task<Post> GetPostById(Guid postId)
    {
      return await _context.Posts
        .Include(x => x.Tags)
        .SingleOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<List<Post>> GetPosts()
    {
      return await _context.Posts
        .Include(x => x.Tags)
        .ToListAsync();
    }

    public async Task<bool> UpdatePost(Post postToUpdate)
    {
      postToUpdate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());
      await AddNewTags(postToUpdate);
      _context.Posts.Update(postToUpdate);

      var count = await _context.SaveChangesAsync();

      return count > 0;
    }

    public async Task<bool> CreatePost(Post postToCreate)
    {
      postToCreate.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

      await AddNewTags(postToCreate);
      await _context.Posts.AddAsync(postToCreate);

      var count = await _context.SaveChangesAsync();
      return count > 0;
    }

    public async Task<bool> UserOwnsPost(Guid postId, string getUserId)
    {
      var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == postId);

      if (post == null ) return false;

      if (post.UserId != getUserId) return false;

      return true;
    }

    public async Task<List<Tag>> GetAllTags()
    {
      return await _context.Tags.AsNoTracking().ToListAsync();
    }

    public async Task<bool> CreateTagAsync(Tag tag)
    {
      tag.Name = tag.Name.ToLower();
      var existingTag = await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tag.Name);
      if (existingTag != null)
        return true;

      await _context.Tags.AddAsync(tag);
      var created = await _context.SaveChangesAsync();
      return created > 0;
    }

    public async Task<bool> DeleteTagAsync(string tagName)
    {
      var tag = await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());

      if (tag == null)
        return true;

      var postTags = await _context.PostTags.Where(x => x.TagName == tagName.ToLower()).ToListAsync();

      _context.PostTags.RemoveRange(postTags);
      _context.Tags.Remove(tag);
      return await _context.SaveChangesAsync() > postTags.Count;
    }

    public async Task<Tag> GetTagByNameAsync(string tagName)
    {
      return await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.Name == tagName.ToLower());
    }

    private async Task AddNewTags(Post post)
    { 
      foreach (var tag in post.Tags)
      {
        var existingTag =
            await _context.Tags.SingleOrDefaultAsync(x =>
                x.Name == tag.TagName);
        if (existingTag != null)
          continue;

        await _context.Tags.AddAsync(new Tag
        { Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = post.UserId });
      }
    }
  }
}
