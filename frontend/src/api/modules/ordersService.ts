import api from '@/api/axios';
import type { IChecklistItem } from '@/interfaces/IChecklistItem';
import type { IServiceOrder } from '@/interfaces/IServiceOrder';

export const ordersService = {
  async getAll(): Promise<IServiceOrder[]> {
    const { data } = await api.get<IServiceOrder[]>(
      '/ServiceOrder'
    );
    return data;
  },

  async getChecklistItems(): Promise<IChecklistItem[]> {
    const { data } = await api.get<IChecklistItem[]>('/ServiceOrder/checklist-items');
    return data;
  }
};
