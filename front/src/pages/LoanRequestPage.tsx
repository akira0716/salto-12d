import React, { useState, useEffect } from 'react';
import { 
  Paper, 
  Box, 
  Typography, 
  TextField, 
  Button, 
  Divider, 
  Grid,
  CircularProgress,
  Alert,
  Breadcrumbs,
  Link
} from '@mui/material';
import { useParams, useNavigate, Link as RouterLink } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import SendIcon from '@mui/icons-material/Send';
import { equipmentApi } from '../api/equipmentApi';
import { loanRequestApi } from '../api/loanRequestApi';
import type { Equipment } from '../types';

const LoanRequestPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  const [equipment, setEquipment] = useState<Equipment | null>(null);
  const [loading, setLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  // フォーム状態
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [purpose, setPurpose] = useState('');

  useEffect(() => {
    if (id) {
      fetchEquipment(Number(id));
    }
  }, [id]);

  const fetchEquipment = async (equipmentId: number) => {
    try {
      const data = await equipmentApi.get(equipmentId);
      setEquipment(data);
      
      // デフォルト日付（明日から1週間など）
      const tomorrow = new Date();
      tomorrow.setDate(tomorrow.getDate() + 1);
      const nextWeek = new Date();
      nextWeek.setDate(tomorrow.getDate() + 7);
      
      setStartDate(tomorrow.toISOString().split('T')[0]);
      setEndDate(nextWeek.toISOString().split('T')[0]);

    } catch (err) {
      setError('備品情報の取得に失敗しました。');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) return;

    setError(null);
    setIsSubmitting(true);

    try {
      await loanRequestApi.create({
        equipmentId: Number(id),
        startDate: startDate,
        endDate: endDate,
        purpose: purpose
      });
      setSuccess(true);
      setTimeout(() => {
        navigate('/mypage');
      }, 2000);
    } catch (err: any) {
      setError(err.message || '申請の送信に失敗しました。');
    } finally {
      setIsSubmitting(false);
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (!equipment) {
    return (
      <Box sx={{ py: 4 }}>
        <Alert severity="error">備品が見つかりませんでした。</Alert>
        <Button startIcon={<ArrowBackIcon />} component={RouterLink} to="/" sx={{ mt: 2 }}>
          一覧に戻る
        </Button>
      </Box>
    );
  }

  return (
    <Box>
      <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 2 }}>
        <Link underline="hover" color="inherit" component={RouterLink} to="/">
          備品一覧
        </Link>
        <Typography color="text.primary">貸出申請</Typography>
      </Breadcrumbs>

      <Typography variant="h4" gutterBottom sx={{ fontWeight: 700, mb: 4 }}>
        貸出申請
      </Typography>

      {success && (
        <Alert severity="success" sx={{ mb: 4 }}>
          申請が完了しました。マイページへ移動します...
        </Alert>
      )}

      {error && (
        <Alert severity="error" sx={{ mb: 4 }}>
          {error}
        </Alert>
      )}

      <Grid container spacing={4}>
        {/* 備品情報カード */}
        <Grid size={{ xs: 12, md: 5 }}>
          <Paper elevation={0} sx={{ p: 4, borderRadius: 3, bgcolor: 'rgba(25, 118, 210, 0.04)', border: '1px solid rgba(25, 118, 210, 0.12)' }}>
            <Typography variant="overline" color="primary" sx={{ fontWeight: 700 }}>
              対象備品
            </Typography>
            <Typography variant="h5" sx={{ fontWeight: 700, mb: 2 }}>
              {equipment.name}
            </Typography>
            <Typography variant="body2" color="textSecondary" sx={{ mb: 3 }}>
              {equipment.description}
            </Typography>
            <Divider sx={{ mb: 2 }} />
            <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
              <Typography variant="body2">カテゴリ:</Typography>
              <Typography variant="body2" sx={{ fontWeight: 600 }}>{equipment.categoryName || equipment.category?.name || '不明'}</Typography>
            </Box>
          </Paper>
        </Grid>

        {/* 申請フォーム */}
        <Grid size={{ xs: 12, md: 7 }}>
          <Paper elevation={0} sx={{ p: 4, borderRadius: 3, border: '1px solid rgba(0, 0, 0, 0.08)' }}>
            <Box component="form" onSubmit={handleSubmit} noValidate>
              <Grid container spacing={3}>
                <Grid size={{ xs: 12, sm: 6 }}>
                  <TextField
                    required
                    fullWidth
                    label="貸出開始希望日"
                    type="date"
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    slotProps={{ inputLabel: { shrink: true } }}
                  />
                </Grid>
                <Grid size={{ xs: 12, sm: 6 }}>
                  <TextField
                    required
                    fullWidth
                    label="返却期限希望日"
                    type="date"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    slotProps={{ inputLabel: { shrink: true } }}
                  />
                </Grid>
                <Grid size={{ xs: 12 }}>
                  <TextField
                    required
                    fullWidth
                    label="利用目的"
                    multiline
                    rows={4}
                    placeholder="例：〇〇プロジェクトの会議で使用するため"
                    value={purpose}
                    onChange={(e) => setPurpose(e.target.value)}
                  />
                </Grid>
                <Grid size={{ xs: 12 }}>
                  <Box sx={{ display: 'flex', gap: 2, pt: 2 }}>
                    <Button
                      fullWidth
                      variant="contained"
                      size="large"
                      type="submit"
                      disabled={isSubmitting || success}
                      startIcon={<SendIcon />}
                      sx={{ py: 1.5, borderRadius: 2 }}
                    >
                      {isSubmitting ? '送信中...' : '申請を送信する'}
                    </Button>
                    <Button
                      fullWidth
                      variant="outlined"
                      size="large"
                      component={RouterLink}
                      to="/"
                      sx={{ py: 1.5, borderRadius: 2 }}
                    >
                      キャンセル
                    </Button>
                  </Box>
                </Grid>
              </Grid>
            </Box>
          </Paper>
        </Grid>
      </Grid>
    </Box>
  );
};

export default LoanRequestPage;
