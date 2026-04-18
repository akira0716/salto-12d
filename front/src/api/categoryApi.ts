import { apiClient } from './client';
import type { Category, CategoryListResponse } from '../types';

export const categoryApi = {
  list: () => apiClient.get<CategoryListResponse>('/categories'),
  
  create: (data: Partial<Category>) => 
    apiClient.post<Category>('/categories', data),
  
  update: (id: number, data: Partial<Category>) => 
    apiClient.put<Category>(`/categories/${id}`, data),
  
  delete: (id: number) => 
    apiClient.delete(`/categories/${id}`),
};
