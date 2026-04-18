const BASE_URL = import.meta.env.VITE_API_URL || '/api/v1';

interface RequestOptions extends RequestInit {
  params?: Record<string, string | number | boolean>;
}

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    if (response.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    const errorBody = await response.json().catch(() => ({}));
    throw new Error(errorBody.message || `API Error: ${response.status} ${response.statusText}`);
  }
  
  if (response.status === 204) {
    return {} as T;
  }
  
  return response.json();
}

export const apiClient = {
  async get<T>(endpoint: string, options: RequestOptions = {}): Promise<T> {
    const fullUrl = `${BASE_URL}${endpoint}`;
    const url = new URL(fullUrl, window.location.origin);
    
    if (options.params) {
      Object.entries(options.params).forEach(([key, value]) => {
        if (value !== undefined && value !== null && String(value) !== 'undefined' && String(value) !== '') {
          url.searchParams.append(key, String(value));
        }
      });
    }

    const token = localStorage.getItem('token');
    const headers = new Headers(options.headers);
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    const response = await fetch(url.toString(), {
      ...options,
      method: 'GET',
      headers,
    });

    return handleResponse<T>(response);
  },

  async post<T>(endpoint: string, body?: any, options: RequestOptions = {}): Promise<T> {
    const url = `${BASE_URL}${endpoint}`;
    const token = localStorage.getItem('token');
    const headers = new Headers(options.headers);
    headers.set('Content-Type', 'application/json');
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    const response = await fetch(url, {
      ...options,
      method: 'POST',
      headers,
      body: JSON.stringify(body),
    });

    return handleResponse<T>(response);
  },

  async put<T>(endpoint: string, body?: any, options: RequestOptions = {}): Promise<T> {
    const url = `${BASE_URL}${endpoint}`;
    const token = localStorage.getItem('token');
    const headers = new Headers(options.headers);
    headers.set('Content-Type', 'application/json');
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    const response = await fetch(url, {
      ...options,
      method: 'PUT',
      headers,
      body: JSON.stringify(body),
    });

    return handleResponse<T>(response);
  },

  async patch<T>(endpoint: string, body?: any, options: RequestOptions = {}): Promise<T> {
    const url = `${BASE_URL}${endpoint}`;
    const token = localStorage.getItem('token');
    const headers = new Headers(options.headers);
    headers.set('Content-Type', 'application/json');
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    const response = await fetch(url, {
      ...options,
      method: 'PATCH',
      headers,
      body: JSON.stringify(body),
    });

    return handleResponse<T>(response);
  },

  async delete<T>(endpoint: string, options: RequestOptions = {}): Promise<T> {
    const url = `${BASE_URL}${endpoint}`;
    const token = localStorage.getItem('token');
    const headers = new Headers(options.headers);
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    const response = await fetch(url, {
      ...options,
      method: 'DELETE',
      headers,
    });

    return handleResponse<T>(response);
  },
};
