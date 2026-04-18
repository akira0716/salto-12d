import React, { useState, useEffect } from 'react';
import { 
  Grid, 
  Card, 
  CardContent, 
  CardActions, 
  Typography, 
  Button, 
  Box, 
  TextField, 
  MenuItem, 
  InputAdornment, 
  Chip,
  Skeleton,
  FormControlLabel,
  Switch
} from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { 
  CheckCircleOutlined,
  Search as SearchIcon,
  FilterList as FilterListIcon
} from '@mui/icons-material';
import { equipmentApi } from '../api/equipmentApi';
import { categoryApi } from '../api/categoryApi';
import type { Equipment, Category } from '../types';

const EquipmentListPage: React.FC = () => {
  const [equipments, setEquipments] = useState<Equipment[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [keyword, setKeyword] = useState('');
  const [categoryId, setCategoryId] = useState<string>('');
  const [showOnlyAvailable, setShowOnlyAvailable] = useState(true);

  useEffect(() => {
    fetchCategories();
  }, []);

  useEffect(() => {
    fetchEquipments();
  }, [categoryId, showOnlyAvailable]);

  const fetchCategories = async () => {
    try {
      const response = await categoryApi.list();
      setCategories(response.categories || []);
    } catch (err) {
      console.error('Failed to fetch categories', err);
    }
  };

  const fetchEquipments = async () => {
    setLoading(true);
    try {
      const response = await equipmentApi.list({
        keyword: keyword || undefined,
        categoryId: categoryId ? Number(categoryId) : undefined,
        status: showOnlyAvailable ? 'available' : undefined
      });
      setEquipments(response.equipments || []);
    } catch (err) {
      console.error('Failed to fetch equipments', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    fetchEquipments();
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'available': return 'success';
      case 'loaned': return 'warning';
      case 'underRepair': return 'error';
      default: return 'default';
    }
  };

  const getStatusLabel = (status: string) => {
    switch (status) {
      case 'available': return '利用可';
      case 'loaned': return '貸出中';
      case 'underRepair': return '修理中';
      case 'disposed': return '廃棄済';
      default: return status;
    }
  };

  return (
    <Box>
      <Typography variant="h4" gutterBottom sx={{ fontWeight: 700, mb: 4 }}>
        備品一覧
      </Typography>

      {/* 検索・フィルターエリア */}
      <Card sx={{ mb: 4, p: 2, borderRadius: 3 }}>
        <Box component="form" onSubmit={handleSearch} sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', alignItems: 'center' }}>
          <TextField
            placeholder="備品名で検索..."
            size="small"
            value={keyword}
            onChange={(e) => setKeyword(e.target.value)}
            sx={{ flexGrow: 1, maxWidth: 400 }}
            slotProps={{
              input: {
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon color="action" />
                  </InputAdornment>
                ),
              }
            }}
          />
          <TextField
            select
            label="カテゴリ"
            size="small"
            value={categoryId}
            onChange={(e) => setCategoryId(e.target.value)}
            sx={{ minWidth: 200 }}
            slotProps={{
              input: {
                startAdornment: (
                  <InputAdornment position="start">
                    <FilterListIcon fontSize="small" color="action" />
                  </InputAdornment>
                ),
              }
            }}
          >
            <MenuItem value="">すべて</MenuItem>
            {categories.map((cat) => (
              <MenuItem key={cat.id} value={cat.id}>{cat.name}</MenuItem>
            ))}
          </TextField>
          <Button variant="contained" type="submit" sx={{ px: 4, borderRadius: 2 }}>
            検索
          </Button>
          
          <Box sx={{ ml: 'auto' }}>
            <FormControlLabel
              control={
                <Switch 
                  checked={showOnlyAvailable} 
                  onChange={(e) => setShowOnlyAvailable(e.target.checked)} 
                />
              }
              label={
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                  <CheckCircleOutlined fontSize="small" color={showOnlyAvailable ? "primary" : "disabled"} />
                  <Typography variant="body2" sx={{ fontWeight: 600 }}>利用可能のみ</Typography>
                </Box>
              }
            />
          </Box>
        </Box>
      </Card>

      {/* 一覧エリア */}
      {loading ? (
        <Grid container spacing={3}>
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <Grid size={{ xs: 12, sm: 6, md: 4 }} key={i}>
              <Skeleton variant="rectangular" height={200} sx={{ borderRadius: 3 }} />
            </Grid>
          ))}
        </Grid>
      ) : equipments.length === 0 ? (
        <Box sx={{ textAlign: 'center', mt: 8 }}>
          <Typography variant="h6" color="textSecondary">
            該当する備品が見つかりませんでした。
          </Typography>
        </Box>
      ) : (
        <Grid container spacing={3}>
          {equipments.map((item) => (
            <Grid size={{ xs: 12, sm: 6, md: 4 }} key={item.id}>
              <Card sx={{ 
                height: '100%', 
                display: 'flex', 
                flexDirection: 'column',
                borderRadius: 3,
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                  transform: 'translateY(-4px)',
                  boxShadow: '0 12px 20px rgba(0,0,0,0.1)'
                }
              }}>
                <CardContent sx={{ flexGrow: 1 }}>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 1 }}>
                    <Chip 
                      label={item.category?.name || 'カテゴリなし'} 
                      size="small" 
                      variant="outlined" 
                      sx={{ color: 'text.secondary', borderColor: 'divider' }}
                    />
                    <Chip 
                      label={getStatusLabel(item.status)} 
                      color={getStatusColor(item.status) as any} 
                      size="small" 
                    />
                  </Box>
                  <Typography variant="h6" component="h2" gutterBottom sx={{ fontWeight: 600 }}>
                    {item.name}
                  </Typography>
                  <Typography variant="body2" color="textSecondary" sx={{ 
                    display: '-webkit-box',
                    WebkitLineClamp: 3,
                    WebkitBoxOrient: 'vertical',
                    overflow: 'hidden',
                    height: '3em'
                  }}>
                    {item.description}
                  </Typography>
                </CardContent>
                <CardActions sx={{ p: 2, pt: 0 }}>
                  <Button 
                    fullWidth 
                    variant="contained" 
                    component={RouterLink} 
                    to={`/equipments/${item.id}/request`}
                    disabled={item.status !== 'available'}
                  >
                    詳細・貸出申請
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
};

export default EquipmentListPage;
