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

export interface ServiceOrderDetail {
  id: number;
  title: string;
  description: string;
  createdAt: string;
  userName?: string; //

  checklists: Array<{
    id: number;
    checklistItemId: number;
    label: string;
    isChecked: boolean;
  }>;

  imageUrls: string[];
}
