"use client";

import { useEffect, useState } from "react";
import { api } from "@/lib/api";
import { formatCurrency, formatDate } from "@/lib/utils";
import type { Order } from "@/lib/types";

export default function AdminOrdersPage() {
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [statusFilter, setStatusFilter] = useState("");

    useEffect(() => {
        setLoading(true);
        api.orders
            .list({ page, pageSize: 10, status: statusFilter || undefined })
            .then((res) => {
                setOrders(res.items);
                setTotalPages(res.totalPages);
            })
            .catch(() => { })
            .finally(() => setLoading(false));
    }, [page, statusFilter]);

    return (
        <div className="p-6 sm:p-8">
            <div className="flex items-center justify-between mb-8">
                <div>
                    <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Manage</p>
                    <h1 className="text-3xl font-bold tracking-tight">Orders</h1>
                </div>
                <select
                    value={statusFilter}
                    onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }}
                    className="px-3 py-2 rounded-xl bg-white/[0.04] border border-white/[0.08] text-sm text-zinc-300 focus:outline-none appearance-none cursor-pointer"
                >
                    <option value="">All Statuses</option>
                    <option value="Pending" className="bg-zinc-900">Pending</option>
                    <option value="Confirmed" className="bg-zinc-900">Confirmed</option>
                    <option value="Processing" className="bg-zinc-900">Processing</option>
                    <option value="Shipped" className="bg-zinc-900">Shipped</option>
                    <option value="Delivered" className="bg-zinc-900">Delivered</option>
                    <option value="Cancelled" className="bg-zinc-900">Cancelled</option>
                </select>
            </div>

            <div className="glass rounded-xl overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-sm">
                        <thead>
                            <tr className="border-b border-white/[0.06]">
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Order #</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Customer</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Date</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Items</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Status</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Payment</th>
                                <th className="text-right px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading
                                ? [...Array(5)].map((_, i) => (
                                    <tr key={i} className="border-b border-white/[0.04]">
                                        <td colSpan={7} className="px-5 py-4"><div className="h-4 bg-white/[0.04] rounded animate-pulse" /></td>
                                    </tr>
                                ))
                                : orders.map((order) => (
                                    <tr key={order.id} className="border-b border-white/[0.04] hover:bg-white/[0.02] transition">
                                        <td className="px-5 py-4 font-mono text-xs text-indigo-400">{order.orderNumber}</td>
                                        <td className="px-5 py-4">
                                            <div>
                                                <p className="text-zinc-200">{order.customerName}</p>
                                                <p className="text-xs text-zinc-500">{order.customerEmail}</p>
                                            </div>
                                        </td>
                                        <td className="px-5 py-4 text-zinc-400 text-xs">{formatDate(order.createdAt)}</td>
                                        <td className="px-5 py-4 text-zinc-400">{order.itemCount}</td>
                                        <td className="px-5 py-4">
                                            <span
                                                className={`inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium ${order.status === "Delivered" ? "bg-emerald-500/10 text-emerald-400"
                                                        : order.status === "Shipped" ? "bg-cyan-500/10 text-cyan-400"
                                                            : order.status === "Cancelled" ? "bg-red-500/10 text-red-400"
                                                                : order.status === "Processing" ? "bg-purple-500/10 text-purple-400"
                                                                    : "bg-amber-500/10 text-amber-400"
                                                    }`}
                                            >
                                                {order.status}
                                            </span>
                                        </td>
                                        <td className="px-5 py-4">
                                            <span
                                                className={`inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium ${order.paymentStatus === "Paid" ? "bg-emerald-500/10 text-emerald-400"
                                                        : order.paymentStatus === "Failed" ? "bg-red-500/10 text-red-400"
                                                            : "bg-zinc-500/10 text-zinc-400"
                                                    }`}
                                            >
                                                {order.paymentStatus}
                                            </span>
                                        </td>
                                        <td className="px-5 py-4 text-right font-medium">{formatCurrency(order.totalAmount)}</td>
                                    </tr>
                                ))}
                        </tbody>
                    </table>
                </div>
            </div>

            {totalPages > 1 && (
                <div className="flex items-center justify-center gap-2 mt-6">
                    <button disabled={page <= 1} onClick={() => setPage(page - 1)}
                        className="px-3 py-1.5 rounded-lg text-xs font-medium bg-white/[0.04] border border-white/[0.08] text-zinc-400 hover:text-white disabled:opacity-30 transition">
                        Previous
                    </button>
                    <span className="text-xs text-zinc-500">{page} / {totalPages}</span>
                    <button disabled={page >= totalPages} onClick={() => setPage(page + 1)}
                        className="px-3 py-1.5 rounded-lg text-xs font-medium bg-white/[0.04] border border-white/[0.08] text-zinc-400 hover:text-white disabled:opacity-30 transition">
                        Next
                    </button>
                </div>
            )}
        </div>
    );
}
