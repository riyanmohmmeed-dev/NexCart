// ═══════════════════════════════════════════════════════════════
//  NexCart — TypeScript Types (mirrors the .NET API DTOs)
// ═══════════════════════════════════════════════════════════════

export interface Product {
    id: string;
    name: string;
    description: string;
    sku: string;
    price: number;
    compareAtPrice: number | null;
    currency: string;
    stockQuantity: number;
    lowStockThreshold: number;
    isActive: boolean;
    isLowStock: boolean;
    discountPercentage: number;
    averageRating: number;
    totalReviews: number;
    categoryId: string;
    categoryName: string;
    imageUrl: string | null;
    createdAt: string;
    updatedAt: string;
}

export interface Category {
    id: string;
    name: string;
    description: string;
    productCount: number;
}

export interface Order {
    id: string;
    orderNumber: string;
    customerId: string;
    customerName: string;
    customerEmail: string;
    status: string;
    paymentStatus: string;
    subTotal: number;
    tax: number;
    shippingCost: number;
    totalAmount: number;
    currency: string;
    itemCount: number;
    items: OrderItem[];
    shippingAddress: Address | null;
    createdAt: string;
    updatedAt: string;
}

export interface OrderItem {
    productId: string;
    productName: string;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
}

export interface Address {
    street: string;
    city: string;
    state: string;
    country: string;
    zipCode: string;
}

export interface DashboardStats {
    totalProducts: number;
    totalOrders: number;
    totalCustomers: number;
    totalRevenue: number;
    currency: string;
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export interface CartItem {
    product: Product;
    quantity: number;
}
