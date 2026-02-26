// ═══════════════════════════════════════════════════════════════
//  NexCart — API Client
// ═══════════════════════════════════════════════════════════════

import type { Product, Category, Order, DashboardStats, PagedResult } from "./types";

const API_BASE = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5119/api";

async function fetcher<T>(url: string, options?: RequestInit): Promise<T> {
    const res = await fetch(`${API_BASE}${url}`, {
        headers: { "Content-Type": "application/json" },
        ...options,
    });

    if (!res.ok) {
        const error = await res.json().catch(() => ({ error: "Request failed" }));
        throw new Error(error.error || `HTTP ${res.status}`);
    }

    return res.json();
}

// ─── Products ───
export const api = {
    products: {
        list: (params?: {
            page?: number;
            pageSize?: number;
            search?: string;
            categoryId?: string;
            sortBy?: string;
            sortDescending?: boolean;
        }) => {
            const query = new URLSearchParams();
            if (params?.page) query.set("page", String(params.page));
            if (params?.pageSize) query.set("pageSize", String(params.pageSize));
            if (params?.search) query.set("search", params.search);
            if (params?.categoryId) query.set("categoryId", params.categoryId);
            if (params?.sortBy) query.set("sortBy", params.sortBy);
            if (params?.sortDescending) query.set("sortDescending", "true");
            return fetcher<PagedResult<Product>>(`/products?${query}`);
        },

        get: (id: string) => fetcher<Product>(`/products/${id}`),

        create: (data: Partial<Product>) =>
            fetcher<Product>("/products", { method: "POST", body: JSON.stringify(data) }),

        update: (id: string, data: Partial<Product>) =>
            fetcher<Product>(`/products/${id}`, { method: "PUT", body: JSON.stringify(data) }),

        delete: (id: string) =>
            fetcher<void>(`/products/${id}`, { method: "DELETE" }),
    },

    orders: {
        list: (params?: { page?: number; pageSize?: number; status?: string }) => {
            const query = new URLSearchParams();
            if (params?.page) query.set("page", String(params.page));
            if (params?.pageSize) query.set("pageSize", String(params.pageSize));
            if (params?.status) query.set("status", params.status);
            return fetcher<PagedResult<Order>>(`/orders?${query}`);
        },

        place: (data: { customerId: string; items: { productId: string; quantity: number }[] }) =>
            fetcher<Order>("/orders", { method: "POST", body: JSON.stringify(data) }),
    },

    categories: {
        list: () => fetcher<Category[]>("/categories"),
    },

    dashboard: {
        stats: () => fetcher<DashboardStats>("/dashboard/stats"),
    },
};
