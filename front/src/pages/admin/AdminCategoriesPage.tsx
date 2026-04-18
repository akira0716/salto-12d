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
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon
} from '@mui/icons-material';
import { categoryApi } from '../../api/categoryApi';
import type { Category } from '../../types';

const AdminCategoriesPage: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // モーダル用
  const [open, setOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<Category | null>(null);
  
  // フォーム状態
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');

  // 削除確認用
  const [deleteId, setDeleteId] = useState<number | null>(null);

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    setLoading(true);
    try {
      const response = await categoryApi.list();
      setCategories(response.categories || []);
    } catch (err) {
      console.error(err);
      setError('データの読み込みに失敗しました。');
    } finally {
      setLoading(false);
    }
  };

  const handleOpen = (item?: Category) => {
    if (item) {
      setEditingItem(item);
      setName(item.name);
      setDescription(item.description || '');
    } else {
      setEditingItem(null);
      setName('');
      setDescription('');
    }
    setOpen(true);
  };

  const handleSave = async () => {
    try {
      if (editingItem) {
        await categoryApi.update(editingItem.id, { name, description });
      } else {
        await categoryApi.create({ name, description });
      }
      setOpen(false);
      fetchCategories();
    } catch (err: any) {
      setError(err.message || '保存に失敗しました。');
    }
  };

  const handleDelete = async () => {
    if (!deleteId) return;
    try {
      await categoryApi.delete(deleteId);
      setDeleteId(null);
      fetchCategories();
    } catch (err: any) {
      setError(err.message || '削除に失敗しました。他のデータに関連付けられている可能性があります。');
      setDeleteId(null);
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
          カテゴリを追加
        </Button>
      </Box>

      {error && <Alert severity="error" sx={{ mb: 4 }} onClose={() => setError(null)}>{error}</Alert>}

      <TableContainer component={Paper} elevation={0} sx={{ border: '1px solid #eee', borderRadius: 2 }}>
        <Table>
          <TableHead sx={{ bgcolor: '#f9fafb' }}>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>カテゴリ名</TableCell>
              <TableCell>説明</TableCell>
              <TableCell align="right">操作</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {categories.map((row) => (
              <TableRow key={row.id}>
                <TableCell>{row.id}</TableCell>
                <TableCell sx={{ fontWeight: 600 }}>{row.name}</TableCell>
                <TableCell>{row.description || '-'}</TableCell>
                <TableCell align="right">
                  <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end' }}>
                    <IconButton color="primary" onClick={() => handleOpen(row)}>
                      <EditIcon />
                    </IconButton>
                    <IconButton color="error" onClick={() => setDeleteId(row.id)}>
                      <DeleteIcon />
                    </IconButton>
                  </Box>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {/* 登録・編集ダイアログ */}
      <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth="xs">
        <DialogTitle>{editingItem ? 'カテゴリの編集' : '新規カテゴリ登録'}</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            margin="normal"
            label="カテゴリ名"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            autoFocus
          />
          <TextField
            fullWidth
            multiline
            rows={3}
            margin="normal"
            label="説明"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </DialogContent>
        <DialogActions sx={{ p: 3 }}>
          <Button onClick={() => setOpen(false)}>キャンセル</Button>
          <Button onClick={handleSave} variant="contained" disabled={!name}>
            保存
          </Button>
        </DialogActions>
      </Dialog>

      {/* 削除確認ダイアログ */}
      <Dialog open={!!deleteId} onClose={() => setDeleteId(null)}>
        <DialogTitle>カテゴリの削除</DialogTitle>
        <DialogContent>
          <Typography>このカテゴリを削除してもよろしいですか？</Typography>
          <Typography variant="body2" color="error" sx={{ mt: 1 }}>
            ※このカテゴリに関連付けられた備品がある場合、削除できません。
          </Typography>
        </DialogContent>
        <DialogActions sx={{ p: 2 }}>
          <Button onClick={() => setDeleteId(null)}>キャンセル</Button>
          <Button onClick={handleDelete} variant="contained" color="error">
            削除する
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default AdminCategoriesPage;
