import { apiClient } from './client';
import type { LoanRequestListResponse } from '../types';

export const loanRequestApi = {
  create: (data: { equipmentId: number; startDate: string; endDate: string; purpose: string }) => 
    apiClient.post<void>('/loan-requests', data),
  
  listMe: () => 
    apiClient.get<LoanRequestListResponse>('/loan-requests/me'),
  
  listAdmin: () => 
    apiClient.get<LoanRequestListResponse>('/admin/loan-requests'),
  
  approve: (id: number) => 
    apiClient.patch<void>(`/admin/loan-requests/${id}/approve`),
  
  reject: (id: number, data: { rejectionReason: string; setEquipmentBroken?: boolean }) => 
    apiClient.patch<void>(`/admin/loan-requests/${id}/reject`, data),
};
