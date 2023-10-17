// CookieService.ts
import Cookies from '../../node_modules/js-cookie';

const TOKEN_KEY = 'jwt_token';

export default class CookieService {
  // Сохранить токен в куки
  static saveToken(token: string): void {
    Cookies.set(TOKEN_KEY, token, { expires: 1, secure: true, sameSite: 'Strict' });
  }

  // Получить токен из куков
  static getToken(): string | null {
    return Cookies.get(TOKEN_KEY) || null;
  }

  // Удалить токен из куков
  static removeToken(): void {
    Cookies.remove(TOKEN_KEY);
  }

  // Сохранить данные в localStorage
  static saveUserData(userData: any): void {
    localStorage.setItem('user_data', JSON.stringify(userData));
  }

  // Получить данные из localStorage
  static getUserData(): any | null {
    const userData = localStorage.getItem('user_data');
    return userData ? JSON.parse(userData) : null;
  }

  // Удалить данные из localStorage
  static removeUserData(): void {
    localStorage.removeItem('user_data');
  }
}
