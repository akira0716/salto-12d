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
  Chip,
  FormControlLabel,
  Switch
} from '@mui/material';
import {
  AssignmentReturn as AssignmentReturnIcon,
  Warning as WarningIcon
} from '@mui/icons-material';
import { loanApi } from '../../api/loanApi';
import type { Loan } from '../../types';
import { formatDate } from '../../utils/dateFormatter';

const AdminLoansPage: React.FC = () => {
  const [loans, setLoans] = useState<Loan[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showOverdueOnly, setShowOverdueOnly] = useState(false);

  useEffect(() => {
    fetchLoans();
  }, [showOverdueOnly]);

  const fetchLoans = async () => {
    setLoading(true);
    try {
      const response = await loanApi.listAdmin({
        overdue: showOverdueOnly || undefined
      });
      setLoans(response.loans || []);
    } catch (err) {
      console.error(err);
      setError('データの読み込みに失敗しました。');
    } finally {
      setLoading(false);
    }
  };

  const handleReturn = async (id: number) => {
    try {
      await loanApi.return(id);
      fetchLoans();
    } catch (err: any) {
      setError(err.message || '返却処理に失敗しました。');
    }
  };

  const isOverdue = (expectedDate: string) => {
    return new Date(expectedDate) < new Date();
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
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center', mb: 3 }}>
        <FormControlLabel
          control={
            <Switch 
              checked={showOverdueOnly} 
              onChange={(e) => setShowOverdueOnly(e.target.checked)} 
              color="error"
            />
          }
          label="延滞のみ表示"
        />
      </Box>

      {error && <Alert severity="error" sx={{ mb: 4 }} onClose={() => setError(null)}>{error}</Alert>}

      {loans.length === 0 ? (
        <Typography color="textSecondary">該当する貸出データはありません。</Typography>
      ) : (
        <TableContainer component={Paper} elevation={0} sx={{ border: '1px solid #eee', borderRadius: 2 }}>
          <Table>
            <TableHead sx={{ bgcolor: '#f9fafb' }}>
              <TableRow>
                <TableCell>利用者</TableCell>
                <TableCell>備品名</TableCell>
                <TableCell>貸出日</TableCell>
                <TableCell>返却予定日</TableCell>
                <TableCell>返却日</TableCell>
                <TableCell>ステータス</TableCell>
                <TableCell align="right">操作</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {loans.map((row) => {
                const overdue = row.status === 'active' && isOverdue(row.dueDate);
                return (
                  <TableRow key={row.id} sx={overdue ? { bgcolor: 'rgba(211, 47, 47, 0.04)' } : {}}>
                    <TableCell>
                      <Typography variant="body2" sx={{ fontWeight: 600 }}>{row.user?.name || row.userName}</Typography>
                    </TableCell>
                    <TableCell>{row.equipment?.name || row.equipmentName}</TableCell>
                    <TableCell>{formatDate(row.loanDate)}</TableCell>
                    <TableCell sx={overdue ? { color: 'error.main', fontWeight: 700 } : {}}>
                      {formatDate(row.dueDate)}
                      {overdue && <WarningIcon fontSize="small" sx={{ ml: 1, verticalAlign: 'middle' }} />}
                    </TableCell>
                    <TableCell>
                      {row.returnDate ? formatDate(row.returnDate) : '-'}
                    </TableCell>
                    <TableCell>
                      {row.status === 'active' ? (
                        <Chip label="貸出中" color={overdue ? "error" : "warning"} size="small" />
                      ) : (
                        <Chip label="返却済" color="success" size="small" variant="outlined" />
                      )}
                    </TableCell>
                    <TableCell align="right">
                      {row.status === 'active' && (
                        <Button 
                          size="small" 
                          variant="outlined" 
                          startIcon={<AssignmentReturnIcon />}
                          onClick={() => handleReturn(row.id)}
                        >
                          返却受領
                        </Button>
                      )}
                    </TableCell>
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Box>
  );
};

export default AdminLoansPage;
