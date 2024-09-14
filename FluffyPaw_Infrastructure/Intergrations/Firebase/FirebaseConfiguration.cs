using Firebase.Auth;
using Firebase.Storage;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Firebase
{
    public class FirebaseConfiguration : IFirebaseConfiguration
    {
        private readonly IConfiguration _configuration;

        public FirebaseConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var apiKey = _configuration["Firebase:ApiKey"];
            var storage = _configuration["Firebase:Storage"];
            var authEmail = _configuration["Firebase:AuthEmail"];
            var authPassword = _configuration["Firebase:AuthPassword"];

            var auth = new FirebaseAuthProvider(new global::Firebase.Auth.FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(authEmail, authPassword);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string folderName;

            var fileExtension = Path.GetExtension(file.FileName);

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    folderName = "images";
                    break;
                case ".docx":
                    folderName = "docx";
                    break;
                case ".ppt":
                case ".pptx":
                    folderName = "ppt";
                    break;
                case ".mp4":
                case ".mov":
                    folderName = "videos";
                    break;
                default:
                    folderName = "other";
                    break;
            }

            var storageProvider = new FirebaseStorage(storage, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            });

            using (var stream = file.OpenReadStream())
            {
                var storageReference = storageProvider.Child(folderName).Child(fileName);
                await storageReference.PutAsync(stream);

                return await storageReference.GetDownloadUrlAsync();
            }
        }
    }
}
