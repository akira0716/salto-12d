import React, { useState, useEffect } from 'react';
import {
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
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  MenuItem
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon
} from '@mui/icons-material';
import { equipmentApi } from '../../api/equipmentApi';
import { categoryApi } from '../../api/categoryApi';
import type { Equipment, Category, EquipmentStatus } from '../../types';

const AdminEquipmentsPage: React.FC = () => {
  const [equipments, setEquipments] = useState<Equipment[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // モーダル用
  const [open, setOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<Equipment | null>(null);

  // フォーム状態
  const [formData, setFormData] = useState({
    name: '',
    categoryId: '',
    description: '',
    status: 'available' as EquipmentStatus
  });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [eqRes, catRes] = await Promise.all([
        equipmentApi.list(),
        categoryApi.list()
      ]);
      setEquipments(eqRes.equipments || []);
      setCategories(catRes.categories || []);
    } catch (err) {
      console.error(err);
      setError('データの読み込みに失敗しました。');
    } finally {
      setLoading(false);
    }
  };

  const handleOpen = (item?: Equipment) => {
    if (item) {
      setEditingItem(item);
      setFormData({
        name: item.name,
        categoryId: item.category ? String(item.category.id) : '',
        description: item.description,
        status: item.status
      });
    } else {
      setEditingItem(null);
      setFormData({
        name: '',
        categoryId: '',
        description: '',
        status: 'available'
      });
    }
    setOpen(true);
  };

  const handleSave = async () => {
    try {
      if (editingItem) {
        await equipmentApi.update(editingItem.id, formData as any);
      } else {
        await equipmentApi.create(formData as any);
      }
      setOpen(false);
      fetchData();
    } catch (err: any) {
      setError(err.message || '保存に失敗しました。');
    }
  };

  const getStatusLabel = (status: string) => {
    switch (status) {
      case 'available': return <Chip label="利用可" color="success" size="small" />;
      case 'loaned': return <Chip label="貸出中" color="warning" size="small" />;
      case 'underRepair': return <Chip label="修理中" color="error" size="small" />;
      case 'disposed': return <Chip label="廃棄済" color="default" size="small" />;
      default: return <Chip label={status} size="small" />;
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
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', alignItems: 'center', mb: 3 }}>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleOpen()}
          sx={{ borderRadius: 2 }}
        >
          備品を追加
        </Button>
      </Box>

      {error && <Alert severity="error" sx={{ mb: 4 }} onClose={() => setError(null)}>{error}</Alert>}

      <TableContainer component={Paper} elevation={0} sx={{ border: '1px solid #eee', borderRadius: 2 }}>
        <Table>
          <TableHead sx={{ bgcolor: '#f9fafb' }}>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>備品名</TableCell>
              <TableCell>カテゴリ</TableCell>
              <TableCell>ステータス</TableCell>
              <TableCell align="right">操作</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {equipments.map((row) => (
              <TableRow key={row.id}>
                <TableCell>{row.id}</TableCell>
                <TableCell sx={{ fontWeight: 600 }}>{row.name}</TableCell>
                <TableCell>{row.categoryName || row.category?.name || '未設定'}</TableCell>
                <TableCell>{getStatusLabel(row.status)}</TableCell>
                <TableCell align="right">
                  <IconButton color="primary" onClick={() => handleOpen(row)}>
                    <EditIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {/* 登録・編集ダイアログ */}
      <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth="sm">
        <DialogTitle>{editingItem ? '備品の編集' : '新規備品登録'}</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            margin="normal"
            label="備品名"
            value={formData.name}
            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
          />
          <TextField
            select
            fullWidth
            margin="normal"
            label="カテゴリ"
            value={formData.categoryId}
            onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
          >
            {categories.map((cat) => (
              <MenuItem key={cat.id} value={String(cat.id)}>{cat.name}</MenuItem>
            ))}
          </TextField>
          <TextField
            select
            fullWidth
            margin="normal"
            label="ステータス"
            value={formData.status}
            onChange={(e) => setFormData({ ...formData, status: e.target.value as EquipmentStatus })}
          >
            <MenuItem value="available">利用可</MenuItem>
            <MenuItem value="loaned">貸出中</MenuItem>
            <MenuItem value="underRepair">修理中</MenuItem>
            <MenuItem value="disposed">廃棄済</MenuItem>
          </TextField>
          <TextField
            fullWidth
            multiline
            rows={3}
            margin="normal"
            label="説明"
            value={formData.description}
            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
          />
        </DialogContent>
        <DialogActions sx={{ p: 3 }}>
          <Button onClick={() => setOpen(false)}>キャンセル</Button>
          <Button onClick={handleSave} variant="contained" disabled={!formData.name || !formData.categoryId}>
            保存
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default AdminEquipmentsPage;
