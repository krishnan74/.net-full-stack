import { Routes } from '@angular/router';
import { VideoListComponent } from './video-list.component';
import { VideoUploadComponent } from './video-upload.component';

export const routes: Routes = [
  { path: '', redirectTo: 'videos', pathMatch: 'full' },
  { path: 'videos', component: VideoListComponent },
  { path: 'upload', component: VideoUploadComponent }
];
