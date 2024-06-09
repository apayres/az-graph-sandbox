using GraphSandbox.Web.Models;

namespace GraphSandbox.Web.Services.Contracts
{
    public interface IProfileService
    {
        Task<ProfileModel> LoadProfile();
    }
}