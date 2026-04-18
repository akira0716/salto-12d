import React, { useState } from 'react';
import { 
  Container, 
  Box, 
  Typography, 
  TextField, 
  Button, 
  Paper, 
  Alert,
  InputAdornment,
  IconButton,
  Link
} from '@mui/material';
import { useNavigate, useLocation, Link as RouterLink } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import LoginIcon from '@mui/icons-material/Login';

const LoginPage: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const from = location.state?.from?.pathname || '/';

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      await login({ email, password });
      navigate(from, { replace: true });
    } catch (err: any) {
      setError(err.message || 'ログインに失敗しました。メールアドレスとパスワードを確認してください。');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Box sx={{ 
      minHeight: '100vh', 
      display: 'flex', 
      alignItems: 'center', 
      justifyContent: 'center',
      background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
    }}>
      <Container maxWidth="xs">
        <Paper elevation={24} sx={{ 
          p: 4, 
          display: 'flex', 
          flexDirection: 'column', 
          alignItems: 'center',
          borderRadius: 4,
          background: 'rgba(255, 255, 255, 0.9)',
          backdropFilter: 'blur(10px)'
        }}>
          <Typography variant="h4" component="h1" gutterBottom sx={{ fontWeight: 800, color: '#4a5568' }}>
            SAVVY LOAN
          </Typography>
          <Typography variant="body1" color="textSecondary" gutterBottom sx={{ mb: 3 }}>
            社内備品貸出管理システム
          </Typography>


          {location.state?.message && <Alert severity="success" sx={{ width: '100%', mb: 2 }}>{location.state.message}</Alert>}
          {error && <Alert severity="error" sx={{ width: '100%', mb: 2 }}>{error}</Alert>}

          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1, width: '100%' }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="メールアドレス"
              name="email"
              autoComplete="email"
              autoFocus
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              variant="outlined"
              sx={{ bgcolor: 'white', borderRadius: 1 }}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="パスワード"
              type={showPassword ? 'text' : 'password'}
              id="password"
              autoComplete="current-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              variant="outlined"
              sx={{ bgcolor: 'white', borderRadius: 1 }}
              slotProps={{
                input: {
                  endAdornment: (
                    <InputAdornment position="end">
                      <IconButton
                        aria-label="toggle password visibility"
                        onClick={() => setShowPassword(!showPassword)}
                        edge="end"
                      >
                        {showPassword ? <VisibilityOff /> : <Visibility />}
                      </IconButton>
                    </InputAdornment>
                  ),
                }
              }}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              disabled={isSubmitting}
              startIcon={<LoginIcon />}
              sx={{ 
                mt: 4, 
                mb: 2, 
                py: 1.5, 
                fontSize: '1rem', 
                fontWeight: 600,
                borderRadius: 2,
                boxShadow: '0 4px 14px 0 rgba(118, 75, 162, 0.39)'
              }}
            >
              {isSubmitting ? 'ログイン中...' : 'ログイン'}
            </Button>
            
            <Box sx={{ mt: 2, textAlign: 'center' }}>
              <Typography variant="caption" color="textSecondary">
                パスワードを忘れた場合はシステム管理者にお問い合わせください。
              </Typography>
            </Box>
            
            <Box sx={{ mt: 2, textAlign: 'center' }}>
              <Link component={RouterLink} to="/signup" variant="body2" sx={{ fontWeight: 600 }}>
                アカウントをお持ちでない方はこちら
              </Link>
            </Box>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
};

export default LoginPage;
