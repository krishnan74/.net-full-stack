using VideoPortalAPI.Repositories;
using VideoPortalAPI.Models;
using VideoPortalAPI.Interfaces;
using VideoPortalAPI.Misc;
using VideoPortalAPI.Models.DTOs.TrainingVideo;
using Azure.Storage.Blobs;

namespace VideoPortalAPI.Services
{
    public class TrainingVideoService : ITrainingVideoService
    {
        private readonly IRepository<int, TrainingVideo> _trainingVideoRepository;
        private readonly TrainingVideoMapper _trainingVideoMapper;
        private readonly BlobContainerClient _containerClient;

        public TrainingVideoService(IConfiguration configuration, IRepository<int, TrainingVideo> trainingVideoRepository)
        {
            _trainingVideoRepository = trainingVideoRepository;
            _trainingVideoMapper = new TrainingVideoMapper();
            var blobStorageConnection = configuration.GetValue<string>("AzureBlob:ConnectionString");
            var containerName = configuration.GetValue<string>("AzureBlob:ContainerName");
            _containerClient = new BlobContainerClient(blobStorageConnection, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<TrainingVideo> AddTrainingVideoAsync(TrainingVideoAddRequestDTO trainingVideo)
        {
            try
            {
                var newTrainingVideo = _trainingVideoMapper.MapVideoAddReqToModel(trainingVideo);
                if (newTrainingVideo == null)
                    throw new Exception("Could not map training video");

                // Upload the video file to Azure Blob Storage
                var blobUrl = await PostFileAsync(trainingVideo.File);
                // Set the Blob URL after uploading
                newTrainingVideo.BlobUrl = blobUrl;

                newTrainingVideo = await _trainingVideoRepository.Add(newTrainingVideo);
                if (newTrainingVideo == null)
                    throw new Exception("Could not add training video");

                return newTrainingVideo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the training video", ex);
            }
        }

        private async Task<string> PostFileAsync(IFormFile file)
        {
            try
            {
                var blobClient = _containerClient.GetBlobClient(file.FileName);
                await blobClient.UploadAsync(file.OpenReadStream(), true);
                return blobClient.Uri.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TrainingVideo> UpdateTrainingVideoAsync(int id, TrainingVideoUpdateRequestDTO trainingVideo)
        {
            try
            {
                var existingVideo = await _trainingVideoRepository.GetById(id);
                if (existingVideo == null)
                    throw new Exception($"Training video with ID {id} not found.");

                var updatedVideo = _trainingVideoMapper.MapVideoUpdateReqToModel(trainingVideo, existingVideo);
                updatedVideo = await _trainingVideoRepository.Update(id, updatedVideo);

                return updatedVideo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the training video", ex);
            }
        }

        public async Task<TrainingVideo> DeleteTrainingVideoAsync(int id)
        {
            try
            {
                var videoToDelete = await _trainingVideoRepository.GetById(id);
                if (videoToDelete == null)
                    throw new Exception($"Training video with ID {id} not found.");

                await _trainingVideoRepository.Delete(id);
                return videoToDelete;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the training video", ex);
            }
        }

        public async Task<List<TrainingVideo>> GetAllTrainingVideosAsync()
        {
            try
            {
                var videos = await _trainingVideoRepository.GetAll();
                return videos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all training videos", ex);
            }
        }

        public async Task<TrainingVideo> GetTrainingVideoByIdAsync(int id)
        {
            try
            {
                var video = await _trainingVideoRepository.GetById(id);
                if (video == null)
                    throw new Exception($"Training video with ID {id} not found.");

                return video;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the training video", ex);
            }
        }
    }
}