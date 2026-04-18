import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box, Container } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import LogoutIcon from '@mui/icons-material/Logout';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';

const Header: React.FC = () => {
  const { user, logout } = useAuth();

  return (
    <AppBar position="sticky" elevation={0} sx={{ 
      background: 'rgba(255, 255, 255, 0.8)', 
      backdropFilter: 'blur(8px)',
      borderBottom: '1px solid rgba(0, 0, 0, 0.12)',
      color: 'text.primary'
    }}>
      <Container maxWidth="lg">
        <Toolbar disableGutters>
          <Typography
            variant="h6"
            noWrap
            component={RouterLink}
            to="/"
            sx={{
              mr: 2,
              fontWeight: 700,
              color: 'primary.main',
              textDecoration: 'none',
              flexGrow: 1
            }}
          >
            SAVVY LOAN
          </Typography>

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            {user ? (
              <>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <AccountCircleIcon fontSize="small" color="action" />
                  <Typography variant="body2" sx={{ fontWeight: 500 }}>
                    {user.name} ({user.role === 'Admin' ? '管理者' : '一般'})
                  </Typography>
                </Box>
                
                {user.role === 'Admin' ? (
                  <Button component={RouterLink} to="/admin/requests" size="small">
                    管理パネル
                  </Button>
                ) : (
                  <Button component={RouterLink} to="/mypage" size="small">
                    マイページ
                  </Button>
                )}

                <Button 
                  onClick={logout} 
                  variant="outlined" 
                  size="small" 
                  startIcon={<LogoutIcon />}
                  sx={{ borderRadius: '20px' }}
                >
                  ログアウト
                </Button>
              </>
            ) : (
              <Button component={RouterLink} to="/login" variant="contained" size="small">
                ログイン
              </Button>
            )}
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default Header;
