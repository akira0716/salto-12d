import { apiClient } from './client';
import type { LoanListResponse } from '../types';

export const loanApi = {
  listAdmin: (params?: { overdue?: boolean; status?: string }) => 
    apiClient.get<LoanListResponse>('/admin/loans', { params }),
  
  listMe: () => 
    apiClient.get<LoanListResponse>('/loans/me'),
  
  return: (id: number) => 
    apiClient.patch<void>(`/admin/loans/${id}/return`),
};
