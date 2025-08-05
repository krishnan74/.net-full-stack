import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

export type ToastSeverity = 'success' | 'info' | 'warn' | 'error';

export interface ToastMessage {
  severity: ToastSeverity;
  summary: string;
  detail: string;
  life?: number;
  key?: string;
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor(private messageService: MessageService) {}

  /**
   * Show a success toast notification
   */
  success(summary: string, detail: string, life: number = 3000): void {
    this.messageService.add({
      severity: 'success',
      summary,
      detail,
      life,
    });
  }

  /**
   * Show an info toast notification
   */
  info(summary: string, detail: string, life: number = 3000): void {
    this.messageService.add({
      severity: 'info',
      summary,
      detail,
      life,
    });
  }

  /**
   * Show a warning toast notification
   */
  warn(summary: string, detail: string, life: number = 4000): void {
    this.messageService.add({
      severity: 'warn',
      summary,
      detail,
      life,
    });
  }

  /**
   * Show an error toast notification
   */
  error(summary: string, detail: string, life: number = 5000): void {
    this.messageService.add({
      severity: 'error',
      summary,
      detail,
      life,
    });
  }

  /**
   * Show a custom toast notification
   */
  show(message: ToastMessage): void {
    this.messageService.add(message);
  }

  /**
   * Clear all toast notifications
   */
  clear(): void {
    this.messageService.clear();
  }

  /**
   * Clear specific toast notifications by key
   */
  clearByKey(key: string): void {
    this.messageService.clear(key);
  }

  // Convenience methods for common scenarios
  /**
   * Show success message for successful operations
   */
  showSuccess(operation: string, details?: string): void {
    const detail = details || `${operation} completed successfully`;
    this.success('Success', detail);
  }

  /**
   * Show error message for failed operations
   */
  showError(operation: string, error?: any): void {
    const detail =
      error?.message || error || `${operation} failed. Please try again.`;
    this.error('Error', detail);
  }

  /**
   * Show info message for informational updates
   */
  showInfo(title: string, message: string): void {
    this.info(title, message);
  }

  /**
   * Show warning message for important notices
   */
  showWarning(title: string, message: string): void {
    this.warn(title, message);
  }

  /**
   * Show message for form validation errors
   */
  showValidationError(field: string, message: string): void {
    this.error('Validation Error', `${field}: ${message}`);
  }

  /**
   * Show message for network/API errors
   */
  showNetworkError(operation: string): void {
    this.error(
      'Network Error',
      `Unable to ${operation}. Please check your connection and try again.`
    );
  }

  /**
   * Show message for authentication errors
   */
  showAuthError(
    message: string = 'Authentication failed. Please login again.'
  ): void {
    this.error('Authentication Error', message);
  }

  /**
   * Show message for permission errors
   */
  showPermissionError(operation: string): void {
    this.error(
      'Permission Denied',
      `You don't have permission to ${operation}.`
    );
  }
}
