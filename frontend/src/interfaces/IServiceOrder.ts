export interface IServiceOrder {
  id: number;
  title: string;
  createdAt: string;
}

export interface CreateOrderPayload {
  title: string;
  description: string;
  checklistItemIds: number[];
}

export interface ServiceOrderResponse {
  id: number;
  title: string;
  description: string;
  userId: number; 
}