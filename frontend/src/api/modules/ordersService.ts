import api from '@/api/axios';
import type { IChecklistItem } from '@/interfaces/IChecklistItem';
import type { CreateOrderPayload, IServiceOrder, ServiceOrderResponse } from '@/interfaces/IServiceOrder';

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
  },

  async create(payload: CreateOrderPayload): Promise<ServiceOrderResponse> {
    const { data } = await api.post<ServiceOrderResponse>('/ServiceOrder', payload);
    return data;
  },

  async uploadImage(orderId: number, file: File): Promise<void> {
    const formData = new FormData();
    formData.append('file', file); 

    await api.post(`/ServiceOrder/${orderId}/images`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/ServiceOrder/${id}`);
  }
};
