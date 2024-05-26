
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace bloggie.web.Reposetory
{
    //this is the iplemtation
    //this reposetory is using IImgReposetory interface to do action
    public class CloudineryImagesReposetory : IImageReposetory
    {
        private readonly IConfiguration configuration;
        //Account is for cloudinary 
        private readonly Account account;

        //IConfiguration is for microsoft read info
        public CloudineryImagesReposetory(IConfiguration configuration)
        {
            //here the info i put it in appsettings im giveing it to account so that i use it
            this.configuration = configuration;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]);
        }
        //IFormFile is for microsoft read info
        public async Task<string> UploadAsync(IFormFile file)
        {
            //here i make the clint that account up there
            var client = new Cloudinary(account);


            //thoese are for uploudiner with some modification!
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            return null;
        }
    }
}
