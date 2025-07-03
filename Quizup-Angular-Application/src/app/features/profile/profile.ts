import { Component } from '@angular/core';
import { ProfileService } from './profile.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css'],
})
export class ProfileComponent {
  userDetails = { name: '', email: '' };
  passwordData = { currentPassword: '', newPassword: '' };
  message = '';

  constructor(private profileService: ProfileService) {}

  updateDetails() {
    this.profileService.updateUserDetails(this.userDetails).subscribe((res) => {
      this.message = 'Profile updated successfully!';
    });
  }

  changePassword() {
    this.profileService.changePassword(this.passwordData).subscribe((res) => {
      this.message = 'Password changed successfully!';
    });
  }
}
