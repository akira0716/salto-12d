import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { AuthProvider } from './contexts/AuthContext';
import Layout from './components/Layout/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import AdminRoute from './components/AdminRoute';

// ページコンポーネントのインポート
import LoginPage from './pages/LoginPage';
import EquipmentListPage from './pages/EquipmentListPage';
import LoanRequestPage from './pages/LoanRequestPage';
import MyPage from './pages/MyPage';
import AdminRequestsPage from './pages/admin/AdminRequestsPage';
import AdminLoansPage from './pages/admin/AdminLoansPage';
import AdminEquipmentsPage from './pages/admin/AdminEquipmentsPage';
import AdminCategoriesPage from './pages/admin/AdminCategoriesPage';
import SignUpPage from './pages/SignUpPage';
import AdminLayout from './components/Admin/AdminLayout';

// プレミアムテーマの定義
const theme = createTheme({
  palette: {
    primary: {
      main: '#6366f1', // Indigo 500
      light: '#818cf8',
      dark: '#4f46e5',
      contrastText: '#fff',
    },
    secondary: {
      main: '#7c3aed', // Violet 600
    },
    background: {
      default: '#f8fafc',
      paper: '#ffffff',
    },
    text: {
      primary: '#1e293b',
      secondary: '#64748b',
    },
  },
  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    h4: {
      fontWeight: 800,
      letterSpacing: '-0.02em',
    },
    h6: {
      fontWeight: 700,
    },
    button: {
      textTransform: 'none',
      fontWeight: 600,
    },
  },
  shape: {
    borderRadius: 12,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          padding: '8px 20px',
        },
        contained: {
          boxShadow: 'none',
          '&:hover': {
            boxShadow: '0 4px 12px rgba(99, 102, 241, 0.3)',
          },
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)',
          border: '1px solid rgba(255, 255, 255, 0.3)',
          transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
          '&:hover': {
            boxShadow: '0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)',
          },
        },
      },
    },
    MuiCssBaseline: {
      styleOverrides: {
        body: {
          background: 'linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%)',
          minHeight: '100vh',
        },
        '.page-enter': { opacity: 0, transform: 'translateY(10px)' },
        '.page-enter-active': { 
          opacity: 1, 
          transform: 'translateY(0)',
          transition: 'opacity 300ms, transform 300ms'
        },
        '.page-exit': { opacity: 1, transform: 'translateY(0)' },
        '.page-exit-active': { 
          opacity: 0, 
          transform: 'translateY(-10px)',
          transition: 'opacity 300ms, transform 300ms'
        },
      },
    },
  },
});

const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/signup" element={<SignUpPage />} />
            
            <Route element={<ProtectedRoute><Layout /></ProtectedRoute>}>
              <Route path="/" element={<EquipmentListPage />} />
              <Route path="/equipments/:id/request" element={<LoanRequestPage />} />
              <Route path="/mypage" element={<MyPage />} />
              
              {/* 管理者用ルート */}
              <Route element={<AdminRoute />}>
                <Route element={<AdminLayout />}>
                  <Route path="/admin/requests" element={<AdminRequestsPage />} />
                  <Route path="/admin/loans" element={<AdminLoansPage />} />
                  <Route path="/admin/equipments" element={<AdminEquipmentsPage />} />
                  <Route path="/admin/categories" element={<AdminCategoriesPage />} />
                </Route>
              </Route>
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </ThemeProvider>
  );
};

export default App;
