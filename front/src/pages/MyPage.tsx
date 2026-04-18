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
  Chip,
  Tabs,
  Tab,
  CircularProgress,
  Alert,
  Tooltip
} from '@mui/material';
import { loanRequestApi } from '../api/loanRequestApi';
import type { LoanRequest } from '../types';
import InfoIcon from '@mui/icons-material/Info';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;
  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ py: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

const MyPage: React.FC = () => {
  const [requests, setRequests] = useState<LoanRequest[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [tabValue, setTabValue] = useState(0);

  useEffect(() => {
    fetchMyRequests();
  }, []);

  const fetchMyRequests = async () => {
    setLoading(true);
    try {
      const response = await loanRequestApi.listMe();
      setRequests(response.loanRequests);
    } catch (err) {
      console.error(err);
      setError('データの読み込みに失敗しました。');
    } finally {
      setLoading(false);
    }
  };

  const currentLoans = requests.filter(r => r.status === 'approved'); // 本来はLoansテーブルから取得すべきだが、簡略化のため
  const history = requests;

  const getStatusChip = (status: string, reason?: string) => {
    switch (status) {
      case 'pending':
        return <Chip label="承認待ち" color="primary" variant="outlined" size="small" />;
      case 'approved':
        return <Chip label="承認済み" color="success" size="small" />;
      case 'rejected':
        return (
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <Chip label="却下" color="error" size="small" />
            {reason && (
              <Tooltip title={reason}>
                <InfoIcon fontSize="small" color="action" />
              </Tooltip>
            )}
          </Box>
        );
      default:
        return <Chip label={status} size="small" />;
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
      <Typography variant="h4" gutterBottom sx={{ fontWeight: 700, mb: 4 }}>
        マイページ
      </Typography>

      {error && <Alert severity="error" sx={{ mb: 4 }}>{error}</Alert>}

      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={tabValue} onChange={(_, val) => setTabValue(val)}>
          <Tab label="現在の利用状況" />
          <Tab label="申請履歴" />
        </Tabs>
      </Box>

      <TabPanel value={tabValue} index={0}>
        {currentLoans.length === 0 ? (
          <Typography color="textSecondary">現在貸出中の備品はありません。</Typography>
        ) : (
          <TableContainer component={Paper} elevation={0} sx={{ border: '1px solid #eee', borderRadius: 2 }}>
            <Table>
              <TableHead sx={{ bgcolor: '#f9fafb' }}>
                <TableRow>
                  <TableCell>備品名</TableCell>
                  <TableCell>開始日</TableCell>
                  <TableCell>返却予定日</TableCell>
                  <TableCell>目的</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {currentLoans.map((row) => (
                  <TableRow key={row.id}>
                    <TableCell sx={{ fontWeight: 600 }}>{row.equipment?.name}</TableCell>
                    <TableCell>{row.startDate}</TableCell>
                    <TableCell>{row.endDate}</TableCell>
                    <TableCell>{row.purpose}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </TabPanel>

      <TabPanel value={tabValue} index={1}>
        {history.length === 0 ? (
          <Typography color="textSecondary">過去の申請履歴はありません。</Typography>
        ) : (
          <TableContainer component={Paper} elevation={0} sx={{ border: '1px solid #eee', borderRadius: 2 }}>
            <Table>
              <TableHead sx={{ bgcolor: '#f9fafb' }}>
                <TableRow>
                  <TableCell>申請日</TableCell>
                  <TableCell>備品名</TableCell>
                  <TableCell>期間</TableCell>
                  <TableCell>ステータス</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {history.map((row) => (
                  <TableRow key={row.id}>
                    <TableCell>{new Date(row.createdAt).toLocaleDateString()}</TableCell>
                    <TableCell sx={{ fontWeight: 600 }}>{row.equipment?.name}</TableCell>
                    <TableCell>{row.startDate} 〜 {row.endDate}</TableCell>
                    <TableCell>{getStatusChip(row.status, row.rejectionReason)}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </TabPanel>
    </Box>
  );
};

export default MyPage;
