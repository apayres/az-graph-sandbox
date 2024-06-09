
using GraphSandbox.Web.Models;
using GraphSandbox.Web.Services.Contracts;
using Microsoft.Graph;
using System.Security.Cryptography;

namespace GraphSandbox.Web.Services
{
    public class ProfileService : IProfileService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public ProfileService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<ProfileModel> LoadProfile()
        {
            var msUser = await _graphServiceClient.Me.Request().Select(user => new
            {
                user.Surname,
                user.GivenName,
                user.Mail
            }).GetAsync();

            var photo = await _graphServiceClient.Me.Photo.Content.Request().GetAsync();
            var photoData = string.Empty;

            using (var reader = new StreamReader(new CryptoStream(photo, new ToBase64Transform(), CryptoStreamMode.Read)))
            {
                photoData = await reader.ReadToEndAsync();
            }             

            return new ProfileModel()
            {
                Email = msUser.Mail,
                FirstName = msUser.GivenName,
                LastName = msUser.Surname,
                PhotoData = photoData
            };
        }
    }
}
