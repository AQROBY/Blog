using Blog.Models;

namespace Blog.Repositories
{
    public class PostRepo
    {
        private List<Post> postRepo;
        private int counter;
        public PostRepo()
        {
            postRepo = new List<Post>();
            counter = 0;
        }

        public Post findById(int id)
        {
            Post post = postRepo.Find(x => x.Id == id);
            if (post != null)
            {
                return post;
            }

            return null;
        }

        public void Add(Post post)
        {
            postRepo.Add(post);
        }


    }
}
