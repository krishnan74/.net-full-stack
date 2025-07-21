import { Component, OnInit } from '@angular/core';
import { VideoService, TrainingVideo } from './video.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-video-list',
  templateUrl: './video-list.component.html',
  styleUrls: ['./video-list.component.css'],
  imports: [CommonModule, FormsModule],
})
export class VideoListComponent implements OnInit {
  videos: TrainingVideo[] = [];
  loading = true;

  constructor(private videoService: VideoService) {}

  ngOnInit() {
    this.videoService.getVideos().subscribe(videos => {
      this.videos = videos;
      this.loading = false;
    });
  }
}
