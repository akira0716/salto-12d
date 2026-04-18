import React from 'react';
import { 
  Box, 
  Container, 
  Tabs, 
  Tab, 
  Paper, 
  Typography, 
  Breadcrumbs, 
  Link 
} from '@mui/material';
import { 
  Outlet, 
  useLocation, 
  useNavigate, 
  Link as RouterLink 
} from 'react-router-dom';
import { 
  NavigateNext as NavigateNextIcon,
  Dashboard as DashboardIcon
} from '@mui/icons-material';

const AdminLayout: React.FC = () => {
  const location = useLocation();
  const navigate = useNavigate();

  // Determine current tab based on path
  const currentPath = location.pathname;
  let tabValue = 0;
  if (currentPath.includes('/admin/loans')) tabValue = 1;
  else if (currentPath.includes('/admin/equipments')) tabValue = 2;
  else if (currentPath.includes('/admin/categories')) tabValue = 3;

  const handleTabChange = (_event: React.SyntheticEvent, newValue: number) => {
    switch (newValue) {
      case 0: navigate('/admin/requests'); break;
      case 1: navigate('/admin/loans'); break;
      case 2: navigate('/admin/equipments'); break;
      case 3: navigate('/admin/categories'); break;
    }
  };

  const getPageTitle = () => {
    switch (tabValue) {
      case 0: return '申請管理';
      case 1: return '全社貸出状況';
      case 2: return '備品マスタ管理';
      case 3: return 'カテゴリ管理';
      default: return '管理パネル';
    }
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Breadcrumbs 
          separator={<NavigateNextIcon fontSize="small" />} 
          aria-label="breadcrumb"
          sx={{ mb: 2 }}
        >
          <Link
            underline="hover"
            color="inherit"
            component={RouterLink}
            to="/"
            sx={{ display: 'flex', alignItems: 'center' }}
          >
            <DashboardIcon sx={{ mr: 0.5 }} fontSize="inherit" />
            ホーム
          </Link>
          <Typography color="text.primary" sx={{ fontWeight: 600 }}>管理者パネル</Typography>
        </Breadcrumbs>
        
        <Typography variant="h4" sx={{ mb: 3, fontWeight: 800 }}>
          {getPageTitle()}
        </Typography>

        <Paper 
          elevation={0} 
          sx={{ 
            borderRadius: 3, 
            border: '1px solid rgba(0,0,0,0.08)',
            overflow: 'hidden',
            background: 'rgba(255, 255, 255, 0.7)',
            backdropFilter: 'blur(20px)',
          }}
        >
          <Tabs
            value={tabValue}
            onChange={handleTabChange}
            indicatorColor="primary"
            textColor="primary"
            variant="scrollable"
            scrollButtons="auto"
            sx={{
              borderBottom: 1,
              borderColor: 'divider',
              px: 2,
              '& .MuiTab-root': {
                py: 2,
                fontSize: '0.95rem',
                fontWeight: 600,
              }
            }}
          >
            <Tab label="申請管理" />
            <Tab label="貸出状況" />
            <Tab label="備品個別管理" />
            <Tab label="カテゴリ管理" />
          </Tabs>
          
          <Box sx={{ p: 3 }}>
            <Outlet />
          </Box>
        </Paper>
      </Box>
    </Container>
  );
};

export default AdminLayout;
