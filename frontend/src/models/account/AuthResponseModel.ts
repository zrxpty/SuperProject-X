export interface AuthResponseModel {
    data: {
      token: string;
      email: string;
      login: string;
      role: string;
    };
    code: number;
    message: string;
  }