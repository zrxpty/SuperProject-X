// CookieService.ts

const TOKEN_KEY = 'jwt_token';

export default class SessionService {
  static saveToken(token: string): void {
    sessionStorage.setItem(TOKEN_KEY, token);
  }

  static getToken(): string | null {
    return sessionStorage.getItem(TOKEN_KEY);
  }

  static removeToken(): void {
    sessionStorage.removeItem(TOKEN_KEY);
  }

  static saveUserData(userData: any): void {
    sessionStorage.setItem('user_data', JSON.stringify(userData));
  }

  static getUserData(): any | null {
    const userData = sessionStorage.getItem('user_data');
    return userData ? JSON.parse(userData) : null;
  }

  static removeUserData(): void {
    sessionStorage.removeItem('user_data');
  }
}
