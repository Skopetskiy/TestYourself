using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestYourself.Domain;

namespace TestYourself.Services
{
  public interface IPostService
  {
    Task<List<Post>> GetPosts();
    Task<Post> GetPostById(Guid Id);

    Task<bool> UpdatePost(Post postToUpdate);
    Task<bool> DeletePost(Guid postToUpdate);
    Task<bool> CreatePost(Post postToCreate);
    Task<bool> UserOwnsPost(Guid postId, string getUserId);

    Task<List<Tag>> GetAllTags();
    Task<bool> CreateTagAsync(Tag tag);
    Task<Tag> GetTagByNameAsync(string tagName);
    Task<bool> DeleteTagAsync(string tagName);
  }
}
