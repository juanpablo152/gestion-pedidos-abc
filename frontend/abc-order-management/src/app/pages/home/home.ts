import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TableComponent } from '../../shared/components/ui/table/table';
import { TableColumn } from '../../shared/types/table/table.interface';
import { UserService } from '../../core/services/user/user.service';
import { MainLayout } from '../../layouts/main-layout/main-layout';
import { UserRegister } from '../../shared/types/user/user.interface';
import { AuthService } from '../../core/auth/auth.service';
import { OrderService } from '../../core/order/order.service';
import { PaymentService } from '../../core/payment/payment.service';
import { OrderResponse } from '../../shared/types/order/order.interface';
import { PaymentResponse } from '../../shared/types/payment/payment.interface';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [TableComponent, MainLayout],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  loading = signal(true);
  users = signal<UserRegister[]>([]);
  orders = signal<OrderResponse[]>([]);
  payments = signal<PaymentResponse[]>([]);
  paymentsWithOrderInfo = signal<any[]>([]);
  pageTitle = signal('Dashboard');

  columnsUsers: TableColumn<any>[] = [
    { key: 'id', label: '#' },
    { key: 'name', label: 'Nombre' },
    { key: 'email', label: 'correo electrónico' },
    { key: 'address', label: 'dirección' },
  ];

  columnsOrders: TableColumn<any>[] = [
    { key: 'id', label: '#' },
    { key: 'productName', label: 'Producto' },
    { key: 'quantity', label: 'Cantidad' },
    { key: 'unitPrice', label: 'Precio unitario', format: { type: 'currency', currency: 'USD' } },
  ];

  columnsPayments: TableColumn<any>[] = [
    { key: 'id', label: '#' },
    { key: 'amount', label: 'Monto', format: { type: 'currency', currency: 'USD' }  },
    { key: 'method', label: 'Método de pago' },
    { key: 'status', label: 'Estado de pago' },
    { key: 'createdAt', label: 'Fecha de creación' },
  ];

  columnsPaymentsWithOrderInfo: TableColumn<any>[] = [
    { key: 'id', label: '#' },
    { key: 'productNames', label: 'Productos' },
    { key: 'userName', label: 'Nombre del usuario' },
    { key: 'userEmail', label: 'Correo electrónico' },
    { key: 'userAddress', label: 'Dirección' },
    { key: 'method', label: 'Método de pago' },
    { key: 'status', label: 'Estado de pago' },
    { key: 'totalAmount', label: 'Total', format: { type: 'currency', currency: 'USD' } },
    { key: 'amount', label: 'Monto Pagado', format: { type: 'currency', currency: 'USD' } },
    { key: 'createdAt', label: 'Fecha de creación' },
  ];

  constructor(
    private userService: UserService,
    private orderService: OrderService,
    private paymentService: PaymentService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
  ) {}

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.pageTitle.set(data['title'] ?? 'Dashboard');
      switch (this.pageTitle()) {
        case 'Dashboard':
          this.getPaymentsWithOrderInfo();
          break;
        case 'Pedidos':
          this.getOrders();
          break;
        case 'Usuarios':
          this.getUsers();
          break;
        case 'Pagos':
          this.getPayments();
          break;
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  async getUsers(): Promise<void> {
    try {
      this.loading.set(true);
      this.users.set(await this.userService.getUsers());
      this.loading.set(false);
    } catch (error) {
      this.loading.set(false);
      console.error(error);
    }
  }

  async getOrders(): Promise<void> {
    try {
      this.loading.set(true);
      const orders = await this.orderService.getAllOrders();
      this.orders.set(
        orders.flatMap((order) =>
          order.items.map((item) => ({ ...order, ...item }))
        )
      );
      this.loading.set(false);
    } catch (error) {
      this.loading.set(false);
      console.error(error);
    }
  }

  async getPaymentsWithOrderInfo(): Promise<void> {
    try {
      this.loading.set(true);
      const data = await this.paymentService.getAllPaymentsWithOrderInfo();
      this.paymentsWithOrderInfo.set(
        data.map((payment) => ({
          id: payment.id,
          productNames: payment.orderItems?.map((i) => i.productName).join(', ') ?? '—',
          userName: payment.userName ?? '—',
          userEmail: payment.userEmail ?? '—',
          userAddress: payment.userAddress ?? '—',
          totalAmount: payment.orderTotalAmount,
          amount: payment.amount,
          method: payment.paymentMethod,
          status: payment.paymentStatus,
          createdAt: payment.createdAt,
        }))
      );
      this.loading.set(false);
    } catch (error) {
      this.loading.set(false);
      console.error(error);
    }
  }

  async getPayments(): Promise<void> {
    try {
      this.loading.set(true);
      this.payments.set(await this.paymentService.getAllPayments());
      this.loading.set(false);
    } catch (error) {
      this.loading.set(false);
      console.error(error);
    }
  }
}
