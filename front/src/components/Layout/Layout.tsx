import React from 'react';
import Header from './Header';
import { Box, Container } from '@mui/material';
import { Outlet } from 'react-router-dom';

const Layout: React.FC = () => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh', bgcolor: '#f8fafc' }}>
      <Header />
      <Container component="main" maxWidth="lg" sx={{ mt: 4, mb: 4, flexGrow: 1 }}>
        <Outlet />
      </Container>
      <Box component="footer" sx={{ py: 3, px: 2, mt: 'auto', textAlign: 'center', borderTop: '1px solid rgba(0, 0, 0, 0.08)' }}>
        <Typography variant="body2" color="text.secondary">
          © {new Date().getFullYear()} 社内備品貸出管理システム SAVVY LOAN
        </Typography>
      </Box>
    </Box>
  );
};

// Typography import was missing in previous thought, adding it here.
import { Typography } from '@mui/material';

export default Layout;
