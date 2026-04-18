import React, { createContext, useState, useEffect, type ReactNode } from 'react';
import type { User } from '../types';
import { authApi } from '../api/authApi';
import { decodeToken, mapPayloadToUser } from '../utils/authHelpers';

interface AuthContextType {
  user: User | null;
  token: string | null;
  isLoading: boolean;
  login: (credentials: { email: string; password: string }) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const savedToken = localStorage.getItem('token');
    
    if (savedToken) {
      setToken(savedToken);
      const payload = decodeToken(savedToken);
      const mappedUser = mapPayloadToUser(payload);
      if (mappedUser) {
        setUser(mappedUser);
      } else {
        localStorage.removeItem('token');
      }
    }
    setIsLoading(false);
  }, []);

  const login = async (credentials: { email: string; password: string }) => {
    setIsLoading(true);
    try {
      const response = await authApi.login(credentials);
      const { token: newToken } = response;
      
      const payload = decodeToken(newToken);
      const newUser = mapPayloadToUser(payload);
      
      if (!newUser) {
        throw new Error('Invalid token received');
      }

      setToken(newToken);
      setUser(newUser);
      
      localStorage.setItem('token', newToken);
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem('token');
    window.location.href = '/login';
  };

  return (
    <AuthContext.Provider value={{ user, token, isLoading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
