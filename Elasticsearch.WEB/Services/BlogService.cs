using Elasticsearch.WEB.Models;
using Elasticsearch.WEB.Repository;
using Elasticsearch.WEB.ViewModel;

namespace Elasticsearch.WEB.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blog;

        public BlogService(BlogRepository blog)
        {
            _blog = blog;
        }
        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            Blog newBlog = new Blog();
            newBlog.UserId = Guid.NewGuid();
            newBlog.Content= model.Content;
            newBlog.Title = model.Title;
            newBlog.Tags = model.Tags.Split(",");  
            
            var data=await _blog.SaveAsync(newBlog);
            return data != null;
        }
        public async Task<List<BlogViewModel>> SearchAsync(string searchText)
        {


            var blogList = await _blog.SearchAsync(searchText);

            return blogList.Select(b => new BlogViewModel()
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                Created = b.Created.ToShortDateString(),
                Tags = String.Join(",", b.Tags),
                UserId = b.UserId.ToString()

            }).ToList();

        }

    }
}
