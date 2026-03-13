export interface OrderResponse {
  id: string;
  userName?: string;
  userEmail?: string;
  userAddress?: string;
  items: OrderItem[];
  totalAmount: number;
  createdAt: Date;
  updatedAt: Date | null;
}

export interface OrderItem {
  productName: string;
  quantity: number;
  unitPrice: number;
}
