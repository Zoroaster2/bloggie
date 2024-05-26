using bloggie.web.Data;
using bloggie.web.Models.Domain;
using bloggie.web.Models.ViewModels;
using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bloggie.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagController : Controller
    {
        private readonly ITagReposetory tagReposetory;

        public AdminTagController(ITagReposetory tagReposetory)
        {
            this.tagReposetory = tagReposetory;
        }

        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        //Maping AddTagRequest To Tag Domain Modal
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagReposetory.AddAsync(tag);

            return RedirectToAction("List");
        }
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            //use db context to read the tags

            var tags = await tagReposetory.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagReposetory.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName

            };


            var updatedTag = await tagReposetory.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show succses
            }
            else
            {
                //show error notofication
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {

            var deletedTag = await tagReposetory.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                //show succses
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }


    }


}

