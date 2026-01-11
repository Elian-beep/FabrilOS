import api from '@/api/axios';
import type { IServiceOrder } from '@/interfaces/IServiceOrder';

export const ordersService = {
  async getAll(): Promise<IServiceOrder[]> {
    const { data } = await api.get<IServiceOrder[]>(
      '/ServiceOrder'
    );
    return data;
  },
};
