import { apiClient } from './client';
import type { Equipment, EquipmentListResponse } from '../types';

export const equipmentApi = {
  list: (params?: { categoryId?: number; keyword?: string; status?: string }) => 
    apiClient.get<EquipmentListResponse>('/equipments', { params }),
  
  get: (id: number) => 
    apiClient.get<Equipment>(`/equipments/${id}`),
  
  create: (data: Partial<Equipment>) => 
    apiClient.post<Equipment>('/equipments', data),
  
  update: (id: number, data: Partial<Equipment>) => 
    apiClient.put<Equipment>(`/equipments/${id}`, data),
};
