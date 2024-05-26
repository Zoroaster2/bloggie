using bloggie.web.Models.Domain;
using bloggie.web.Models.ViewModels;
using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace bloggie.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostController : Controller
    {
        private readonly ITagReposetory tagReposetory;
        private readonly IBlogPostReposetory blogPostReposetory;

        public AdminBlogPostController(ITagReposetory tagReposetory, IBlogPostReposetory blogPostReposetory)
        {
            this.tagReposetory = tagReposetory;
            this.blogPostReposetory = blogPostReposetory;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tag = await tagReposetory.GetAllAsync();

            var modal = new AddBlogPostRequest
            {
                Tags = tag.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };



            return View(modal);
        }
        //Now We Must Inject The BlogPost and Up Here We Assign it 
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Map ViewModel To Domaim Model
            //Assinged From AddBlogPostRequest
            var blogpost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible

            };

            //Map Tag From Selected tag
            var selectedTag = new List<Tag>();
            foreach (var SelectedtagId in addBlogPostRequest.SelectedTags)
            {
                //Must Convert Guid to var
                var selectedTagsAsGuid = Guid.Parse(SelectedtagId);

                //Search The DataBase Throw Reposetory
                var exsitingTag = await tagReposetory.GetAsync(selectedTagsAsGuid);
                //it looping throw selected tag to find it
                if (exsitingTag != null)
                {
                    selectedTag.Add(exsitingTag);

                }

            }

            //Maping tags back to domain model
            blogpost.Tags = selectedTag;

            await blogPostReposetory.AddAsync(blogpost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //call reposetory to get data
            var blogpost = await blogPostReposetory.GetAllasync();

            return View(blogpost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //retrive the result from reposetory
            var blogPost = await blogPostReposetory.GetAsync(id);

            //Map
            var tagsDomainModel = await tagReposetory.GetAllAsync();

            if (blogPost != null)
            {
                //map the domain model to view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => id.ToString()).ToArray()

                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        //For Edit We Need The Model That We Use In The View
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //Map View Model Back To Domain Model
            var blogPostDomainInModal = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible
            };

            //Map Tags Into Domain Model
            //we must loop throw strings in array and use tagreposetory to check if the id existing that
            //we add its property to blog post
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                //tag is guid here
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    //tag in get is that guid id
                    var foundTag = await tagReposetory.GetAsync(tag);
                    {
                        if (foundTag != null)
                        {
                            selectedTags.Add(foundTag);
                        }
                    }
                }
            }
            blogPostDomainInModal.Tags = selectedTags;

            //Submite information to reposetory to Update

            var updatedBlog = await blogPostReposetory.UpdateAsync(blogPostDomainInModal);
            if (updatedBlog != null)
            {
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");

        }

        //its post method cus we postin information from model
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {

            //Talk to reposetory To Reposetory to delete blogPost and tags
            var deletedBlogPost = await blogPostReposetory.DeleteAsync(editBlogPostRequest.Id);

            //display the response
            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            else
            {
                //edit here it take object and id here is the routh
                return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
            }


            
        }






    }
}



//Reposetory Only deals With Domain Model
//same thing we did in controller but that was for viewModel but this is actuall updating dataBase
