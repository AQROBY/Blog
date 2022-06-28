using Blog.Models;

namespace Blog.Repositories
{
    public class PostRepo
    {
        private List<Post> postRepo;
        public PostRepo()
        {
            postRepo = new List<Post>();
        }

        public void Add(Post post)
        {
            postRepo.Add(post);
        }


    }
}
