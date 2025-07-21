using VideoPortalAPI.Models;
using VideoPortalAPI.Models.DTOs.TrainingVideo;

namespace VideoPortalAPI.Misc
{
    public class TrainingVideoMapper
    {
        public TrainingVideo? MapVideoAddReqToModel(TrainingVideoAddRequestDTO addRequestDTO)
        {
            TrainingVideo trainingVideo = new();
            trainingVideo.Title = addRequestDTO.Title;
            trainingVideo.Description = addRequestDTO.Description;
            trainingVideo.UploadDate = DateTime.UtcNow;

            return trainingVideo;
        }

        public TrainingVideo? MapVideoUpdateReqToModel(TrainingVideoUpdateRequestDTO updateRequestDTO, TrainingVideo existingVideo)
        {
            if (existingVideo == null) return null;

            existingVideo.Title = updateRequestDTO.Title;
            existingVideo.Description = updateRequestDTO.Description;
            return existingVideo;
        }
    }
}
