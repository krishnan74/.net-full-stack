import { Component } from '@angular/core';
import { VideoService } from './video.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-video-upload',
  templateUrl: './video-upload.component.html',
  styleUrls: ['./video-upload.component.css'],
  imports: [CommonModule, FormsModule],

})
export class VideoUploadComponent {
  title = '';
  description = '';
  file: File | null = null;
  uploading = false;
  error = '';

  constructor(private videoService: VideoService, private router: Router) {}

  onFileChange(event: any) {
    this.file = event.target.files[0];
  }

  upload() {
    if (!this.title || !this.description || !this.file) {
      this.error = 'All fields are required.';
      return;
    }
    this.uploading = true;
    const formData = new FormData();
    formData.append('title', this.title);
    formData.append('description', this.description);
    formData.append('file', this.file);
    this.videoService.uploadVideo(formData).subscribe({
      next: () => {
        this.uploading = false;
        this.router.navigate(['/videos']);
      },
      error: err => {
        this.uploading = false;
        this.error = 'Upload failed.';
      }
    });
  }
}
