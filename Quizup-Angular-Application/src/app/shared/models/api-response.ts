export class ApiResponse<T> {
  id: string;
  data: T;
  message: string;
  success: boolean;
  errors: {};

  constructor(id: string, data: T, message: string, success: boolean, errors: {}) {
    this.id = id;
    this.data = data;
    this.message = message;
    this.success = success;
    this.errors = {};
  }
}