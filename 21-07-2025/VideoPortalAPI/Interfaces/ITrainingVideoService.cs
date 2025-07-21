using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalAPI.Models;
using VideoPortalAPI.Models.DTOs.TrainingVideo;

namespace VideoPortalAPI.Interfaces
{
    public interface ITrainingVideoService
    {
        public Task<TrainingVideo> AddTrainingVideoAsync(TrainingVideoAddRequestDTO trainingVideo);
        public Task<TrainingVideo> UpdateTrainingVideoAsync(int id, TrainingVideoUpdateRequestDTO trainingVideo);
        public Task<TrainingVideo> DeleteTrainingVideoAsync(int id);
        public Task<List<TrainingVideo>> GetAllTrainingVideosAsync();
        public Task<TrainingVideo> GetTrainingVideoByIdAsync(int id);
    }
}
