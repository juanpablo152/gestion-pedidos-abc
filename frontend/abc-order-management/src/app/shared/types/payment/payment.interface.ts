import { OrderItem } from "../order/order.interface";

export interface PaymentResponse {
  id: string;
  amount: number;
  paymentMethod: string;
  paymentStatus: string;
  createdAt: Date;
  updatedAt: Date | null;
  orderId: string;
  orderTotalAmount?: number;
  orderItems?: OrderItem[];
  userName?: string;
  userEmail?: string;
  userAddress?: string;
}

