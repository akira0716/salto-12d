export type Role = 'Admin' | 'Employee';

export interface User {
  id: number;
  name: string;
  email: string;
  role: Role;
}

export type EquipmentStatus = 'available' | 'loaned' | 'in_repair' | 'disposed';

export interface Category {
  id: number;
  name: string;
  description?: string;
}

export interface Equipment {
  id: number;
  name: string;
  category: Category;
  status: EquipmentStatus;
  description: string;
}

export type LoanRequestStatus = 'pending' | 'approved' | 'rejected';

export interface LoanRequest {
  id: number;
  equipmentId: number;
  equipment?: Equipment;
  userId: number;
  user?: User;
  startDate: string;
  endDate: string;
  purpose: string;
  status: LoanRequestStatus;
  rejectionReason?: string;
  createdAt: string;
}

export type LoanStatus = 'active' | 'returned';

export interface Loan {
  id: number;
  requestId: number;
  userId: number;
  user?: User;
  equipmentId: number;
  equipment?: Equipment;
  startDate: string;
  expectedReturnDate: string;
  actualReturnDate?: string;
  status: LoanStatus;
}

// API Response Types
export interface LoginResponse {
  token: string;
}

export interface EquipmentListResponse {
  equipments: Equipment[];
}

export interface CategoryListResponse {
  categories: Category[];
}

export interface LoanRequestListResponse {
  loanRequests: LoanRequest[];
}

export interface LoanListResponse {
  loans: Loan[];
}
