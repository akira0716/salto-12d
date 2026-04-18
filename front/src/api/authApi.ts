import { apiClient } from './client';
import type { LoginResponse } from '../types';

export const authApi = {
  login: (credentials: { email: string; password: string }) => 
    apiClient.post<LoginResponse>('/auth/login', credentials),
  
  signup: (userData: any) => 
    apiClient.post<any>('/auth/signup', userData),
};
