import { Component } from '@angular/core';
import { Toast } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [Toast, CommonModule],
  template: `
    <p-toast
      position="top-center"
      [baseZIndex]="1000"
      [showTransformOptions]="'translateY(-100%) scale(0.8)'"
      [hideTransformOptions]="'translateY(-100%) scale(0.8)'"
      [showTransitionOptions]="'400ms cubic-bezier(0.68, -0.55, 0.265, 1.55)'"
      [hideTransitionOptions]="'300ms ease-in'"
      [autoZIndex]="true"
      [styleClass]="'custom-toast'"
    ></p-toast>
  `,
  styles: [
    `
      /* Override PrimeNG default styles with higher specificity */
      :host ::ng-deep .custom-toast.p-toast {
        top: 80px !important;
        max-width: 400px !important;
        width: 100% !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message {
        border-radius: 16px !important;
        box-shadow: 0 8px 32px rgba(0, 0, 0, 0.12) !important;
        border: none !important;
        margin-bottom: 12px !important;
        backdrop-filter: blur(10px) !important;
        position: relative !important;
        overflow: hidden !important;
        transition: all 0.3s ease !important;
        padding: 0 !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message:hover {
        transform: translateY(-2px) !important;
        box-shadow: 0 12px 40px rgba(0, 0, 0, 0.15) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message::before {
        content: '' !important;
        position: absolute !important;
        top: 0 !important;
        left: 0 !important;
        right: 0 !important;
        height: 3px !important;
        background: linear-gradient(
          90deg,
          var(--accent-primary),
          var(--accent-secondary)
        ) !important;
        z-index: 1 !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-success {
        background: linear-gradient(
          135deg,
          var(--status-active-bg) 0%,
          rgba(74, 222, 128, 0.05) 100%
        ) !important;
        color: var(--status-active-text) !important;
        border-left: 4px solid var(--accent-success) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-success::before {
        background: linear-gradient(
          90deg,
          var(--accent-success),
          #22c55e
        ) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-error {
        background: linear-gradient(
          135deg,
          var(--status-inactive-bg) 0%,
          rgba(248, 113, 113, 0.05) 100%
        ) !important;
        color: var(--status-inactive-text) !important;
        border-left: 4px solid var(--accent-error) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-error::before {
        background: linear-gradient(
          90deg,
          var(--accent-error),
          #ef4444
        ) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-warn {
        background: linear-gradient(
          135deg,
          rgba(251, 191, 36, 0.1) 0%,
          rgba(245, 158, 11, 0.05) 100%
        ) !important;
        color: var(--accent-warning) !important;
        border-left: 4px solid var(--accent-warning) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-warn::before {
        background: linear-gradient(
          90deg,
          var(--accent-warning),
          #f59e0b
        ) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-info {
        background: linear-gradient(
          135deg,
          rgba(96, 165, 250, 0.1) 0%,
          rgba(59, 130, 246, 0.05) 100%
        ) !important;
        color: var(--accent-info) !important;
        border-left: 4px solid var(--accent-info) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-info::before {
        background: linear-gradient(
          90deg,
          var(--accent-info),
          #3b82f6
        ) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-icon {
        color: inherit !important;
        font-size: 1.5rem !important;
        margin-right: 12px !important;
        display: flex !important;
        align-items: center !important;
        justify-content: center !important;
        width: 24px !important;
        height: 24px !important;
        border-radius: 50% !important;
        background: rgba(255, 255, 255, 0.2) !important;
        backdrop-filter: blur(5px) !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-content {
        padding: 16px 20px !important;
        display: flex !important;
        align-items: flex-start !important;
        gap: 12px !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-text {
        flex: 1 !important;
        display: flex !important;
        flex-direction: column !important;
        gap: 4px !important;
      }

      :host ::ng-deep .custom-toast .p-toast-summary {
        font-weight: 700 !important;
        font-size: 1rem !important;
        margin-bottom: 2px !important;
        letter-spacing: 0.025em !important;
      }

      :host ::ng-deep .custom-toast .p-toast-detail {
        font-size: 0.875rem !important;
        line-height: 1.5 !important;
        opacity: 0.9 !important;
        font-weight: 400 !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-close-icon {
        color: inherit !important;
        font-size: 1.1rem !important;
        opacity: 0.7 !important;
        cursor: pointer !important;
        transition: all 0.2s ease !important;
        padding: 4px !important;
        border-radius: 50% !important;
        display: flex !important;
        align-items: center !important;
        justify-content: center !important;
        width: 28px !important;
        height: 28px !important;
      }

      :host ::ng-deep .custom-toast .p-toast-message-close-icon:hover {
        opacity: 1 !important;
        background: rgba(255, 255, 255, 0.1) !important;
        transform: scale(1.1) !important;
      }

      /* Custom icons for different message types */
      :host
        ::ng-deep
        .custom-toast
        .p-toast-message-success
        .p-toast-message-icon::before {

        font-weight: bold !important;
      }

      :host
        ::ng-deep
        .custom-toast
        .p-toast-message-error
        .p-toast-message-icon::before {
       
        font-weight: bold !important;
      }

      :host
        ::ng-deep
        .custom-toast
        .p-toast-message-warn
        .p-toast-message-icon::before {

        font-weight: bold !important;
      }

      :host
        ::ng-deep
        .custom-toast
        .p-toast-message-info
        .p-toast-message-icon::before {

        font-weight: bold !important;
      }

      /* Animation for new toasts */
      :host ::ng-deep .custom-toast .p-toast-message {
        animation: slideInFromTop 0.4s cubic-bezier(0.68, -0.55, 0.265, 1.55) !important;
      }

      @keyframes slideInFromTop {
        0% {
          opacity: 0;
          transform: translateY(-100%) scale(0.8);
        }
        100% {
          opacity: 1;
          transform: translateY(0) scale(1);
        }
      }

      /* Progress bar animation */
      :host ::ng-deep .custom-toast .p-toast-message::after {
        content: '' !important;
        position: absolute !important;
        bottom: 0 !important;
        left: 0 !important;
        height: 2px !important;
        background: currentColor !important;
        opacity: 0.3 !important;
        animation: progressBar 3s linear forwards !important;
        z-index: 1 !important;
      }

      @keyframes progressBar {
        0% {
          width: 100%;
        }
        100% {
          width: 0%;
        }
      }

      /* Responsive design */
      @media (max-width: 480px) {
        :host ::ng-deep .custom-toast.p-toast {
          max-width: 90vw !important;
          margin: 0 16px !important;
        }

        :host ::ng-deep .custom-toast .p-toast-message-content {
          padding: 12px 16px !important;
        }

        :host ::ng-deep .custom-toast .p-toast-summary {
          font-size: 0.9rem !important;
        }

        :host ::ng-deep .custom-toast .p-toast-detail {
          font-size: 0.8rem !important;
        }
      }

      /* Dark mode enhancements */
      :host ::ng-deep .dark .custom-toast .p-toast-message {
        box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3) !important;
      }

      :host ::ng-deep .dark .custom-toast .p-toast-message:hover {
        box-shadow: 0 12px 40px rgba(0, 0, 0, 0.4) !important;
      }

      :host ::ng-deep .dark .custom-toast .p-toast-message-icon {
        background: rgba(255, 255, 255, 0.1) !important;
      }

      :host ::ng-deep .dark .custom-toast .p-toast-message-close-icon:hover {
        background: rgba(255, 255, 255, 0.15) !important;
      }
    `,
  ],
})
export class ToastComponent {
  constructor(public messageService: MessageService) {}
}
