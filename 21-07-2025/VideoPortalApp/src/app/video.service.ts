import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

export interface TrainingVideo {
  id: number;
  title: string;
  description: string;
  uploadDate: string;
  blobUrl: string;
}

@Injectable({ providedIn: 'root' })
export class VideoService {
  private apiUrl = ' http://localhost:5166/api/videos';

  constructor(private http: HttpClient) {}

  getVideos(): Observable<TrainingVideo[]> {
    console.log('Fetching videos from:', this.apiUrl);
    const response = this.http.get<TrainingVideo[]>(this.apiUrl).pipe(
        map(videos => {
            console.log('Videos fetched:', videos);
            return videos;
        })
    );
    return response;
  }

  uploadVideo(formData: FormData): Observable<any> {
    return this.http.post(this.apiUrl + '/upload', formData);
  }
}
