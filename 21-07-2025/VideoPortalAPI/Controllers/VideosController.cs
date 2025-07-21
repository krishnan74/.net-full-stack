using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoPortalAPI.Interfaces;
using VideoPortalAPI.Models;
using VideoPortalAPI.Models.DTOs.TrainingVideo;


namespace VideoPortalAPI.Controllers
{

    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ITrainingVideoService _trainingVideoService;

        public VideosController(ITrainingVideoService trainingVideoService)
        {
            _trainingVideoService = trainingVideoService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo([FromForm] TrainingVideoAddRequestDTO uploadDto)
        {
            if (uploadDto.File == null || uploadDto.File.Length == 0)
                return BadRequest("No video file uploaded.");

            var result = await _trainingVideoService.AddTrainingVideoAsync(uploadDto);
            if (result == null)
            return BadRequest("Failed to upload video.");

            return Created("", result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingVideo(int id, [FromBody] TrainingVideoUpdateRequestDTO trainingVideo)
        {
            var result = await _trainingVideoService.UpdateTrainingVideoAsync(id, trainingVideo);
            if (result == null)
                return NotFound($"Training video with ID {id} not found.");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingVideo(int id)
        {
            var result = await _trainingVideoService.DeleteTrainingVideoAsync(id);
            if (result == null)
                return NotFound($"Training video with ID {id} not found.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingVideo>>> GetTrainingVideos()
        {
            var videos = await _trainingVideoService.GetAllTrainingVideosAsync();
            return Ok(videos);
        }

        [HttpGet("{id}/stream")]
        public async Task<IActionResult> GetTrainingVideoById(int id)
        {
            var video = await _trainingVideoService.GetTrainingVideoByIdAsync(id);
            if (video == null)
                return NotFound($"Training video with ID {id} not found.");

            return Ok(video);
        }
    }
}