import api from '@/api/axios';
import type { 
  LoginRequest, 
  RefreshTokenRequest, 
  AuthResponse 
} from '@/interfaces/IAuth';

export const authService = {
  async login(payload: LoginRequest): Promise<AuthResponse> {
    const { data } = await api.post<AuthResponse>('/Auth/login', payload);
    return data;
  },
  async refreshToken(payload: RefreshTokenRequest): Promise<AuthResponse> {
    const { data } = await api.post<AuthResponse>('/Auth/refresh-token', payload);
    return data;
  }
};