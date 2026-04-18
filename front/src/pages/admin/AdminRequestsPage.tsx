import React, { useState, useEffect } from 'react';
import { 
  Typography, 
  Box, 
  Paper, 
  Table, 
  TableBody, 
  TableCell, 
  TableContainer, 
  TableHead, 
  TableRow,
  Button,
  CircularProgress,
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  FormControlLabel,
  Checkbox
} from '@mui/material';
import {
  Check as CheckIcon,
  Close as CloseIcon
} from '@mui/icons-material';
import { loanRequestApi } from '../../api/loanRequestApi';
import type { LoanRequest } from '../../types';
import { formatDate } from '../../utils/dateFormatter';

const AdminRequestsPage: React.FC = () => {
  const [requests, setRequests] = useState<LoanRequest[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // 却下ダイアログ用
  const [rejectId, setRejectId] = useState<number | null>(null);
  const [rejectionReason, setRejectionReason] = useState('');
  const [setBroken, setSetBroken] = useState(false);

  useEffect(() => {
    fetchRequests();
  }, []);

  const fetchRequests = async () => {
    setLoading(true);
    try {
      const response = await loanRequestApi.listAdmin();
      setRequests((response.loanRequests || []).filter(r => r.status === 'pending'));
    } catch (err) {
      console.error(err);
      setError('データの読み込みに失敗しました。');
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (id: number) => {
    try {
      await loanRequestApi.approve(id);
      fetchRequests();
    } catch (err: any) {
      setError(err.message || '承認に失敗しました。');
    }
  };

  const handleRejectSubmit = async () => {
    if (!rejectId) return;
    try {
      await loanRequestApi.reject(rejectId, {
        rejectionReason: rejectionReason,
        setEquipmentBroken: setBroken
      });
      setRejectId(null);
      setRejectionReason('');
      setSetBroken(false);
      fetchRequests();
    } catch (err: any) {
      setError(err.message || '却下に失敗しました。');
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }
  return (
    <Box>
      {error && <Alert severity="error" sx={{ mb: 4 }} onClose={() => setError(null)}>{error}</Alert>}

      {requests.length === 0 ? (
        <Typography color="textSecondary">未処理の申請はありません。</Typography>
      ) : (
        <TableContainer component={Paper} elevation={0} sx={{ border: '1px solid #eee', borderRadius: 2 }}>
          <Table>
            <TableHead sx={{ bgcolor: '#f9fafb' }}>
              <TableRow>
                <TableCell>申請者</TableCell>
                <TableCell>備品名</TableCell>
                <TableCell>希望期間</TableCell>
                <TableCell>利用目的</TableCell>
                <TableCell align="right">操作</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {requests.map((row) => (
                <TableRow key={row.id}>
                  <TableCell>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>{row.user?.name || row.userName || `User ID: ${row.userId}`}</Typography>
                    <Typography variant="caption" color="textSecondary">{row.user?.email || ''}</Typography>
                  </TableCell>
                  <TableCell>{row.equipment?.name || row.equipmentName || `Equipment ID: ${row.equipmentId}`}</TableCell>
                  <TableCell>{formatDate(row.startDate)} 〜 {formatDate(row.endDate)}</TableCell>
                  <TableCell sx={{ maxWidth: 300 }}>{row.purpose}</TableCell>
                  <TableCell align="right">
                    <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end' }}>
                      <Button 
                        size="small" 
                        variant="contained" 
                        color="success" 
                        startIcon={<CheckIcon />}
                        onClick={() => handleApprove(row.id)}
                      >
                        承認
                      </Button>
                      <Button 
                        size="small" 
                        variant="outlined" 
                        color="error" 
                        startIcon={<CloseIcon />}
                        onClick={() => setRejectId(row.id)}
                      >
                        却下
                      </Button>
                    </Box>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      {/* 却下理由ダイアログ */}
      <Dialog open={!!rejectId} onClose={() => setRejectId(null)} fullWidth maxWidth="sm">
        <DialogTitle>申請の却下</DialogTitle>
        <DialogContent>
          <Typography variant="body2" gutterBottom sx={{ mt: 1 }}>
            却下理由を記入してください。この理由は申請者に通知（表示）されます。
          </Typography>
          <TextField
            fullWidth
            multiline
            rows={3}
            margin="normal"
            label="却下理由"
            value={rejectionReason}
            onChange={(e) => setRejectionReason(e.target.value)}
          />
          <FormControlLabel
            control={
              <Checkbox 
                checked={setBroken} 
                onChange={(e) => setSetBroken(e.target.checked)} 
              />
            }
            label="同時に対象備品のステータスを「修理中」に変更する"
          />
        </DialogContent>
        <DialogActions sx={{ p: 3 }}>
          <Button onClick={() => setRejectId(null)}>キャンセル</Button>
          <Button 
            onClick={handleRejectSubmit} 
            variant="contained" 
            color="error"
            disabled={!rejectionReason}
          >
            却下を確定する
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default AdminRequestsPage;
