using VideoPortalAPI.Contexts;
using VideoPortalAPI.Interfaces;
using VideoPortalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoPortalAPI.Repositories
{
    public  class TrainingVideoRepository : Repository<int, TrainingVideo>
    {
        public TrainingVideoRepository(VideoPortalContext videoPortalContext) : base(videoPortalContext)
        {
        }

        public override async Task<TrainingVideo> GetById(int key)
        {
            var video = await _videoPortalContext.TrainingVideos.SingleOrDefaultAsync(p => p.Id == key);

            return video ?? throw new KeyNotFoundException($"No Training Video with the given ID: {key}");
        }

        public override async Task<IEnumerable<TrainingVideo>> GetAll()
        {
            var videos = _videoPortalContext.TrainingVideos;

            if (videos.Count() == 0)
                return Enumerable.Empty<TrainingVideo>();
            return (await videos.ToListAsync());
        }
    }
}