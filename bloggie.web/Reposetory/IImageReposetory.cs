namespace bloggie.web.Reposetory
{
    public interface IImageReposetory
    {
        Task<string>  UploadAsync(IFormFile file);
    }
}
