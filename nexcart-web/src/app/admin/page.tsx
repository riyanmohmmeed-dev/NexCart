"use client";

import { useEffect, useState } from "react";
import { DollarSign, Package, ShoppingCart, Users } from "lucide-react";
import { api } from "@/lib/api";
import { formatCurrency } from "@/lib/utils";
import type { DashboardStats, Order } from "@/lib/types";

export default function AdminDashboardPage() {
    const [stats, setStats] = useState<DashboardStats | null>(null);
    const [recentOrders, setRecentOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        Promise.all([api.dashboard.stats(), api.orders.list({ pageSize: 5 })])
            .then(([s, o]) => {
                setStats(s);
                setRecentOrders(o.items);
            })
            .catch(() => { })
            .finally(() => setLoading(false));
    }, []);

    const statCards = stats
        ? [
            { label: "Revenue", value: formatCurrency(stats.totalRevenue), icon: DollarSign, color: "text-emerald-400", bg: "bg-emerald-500/10" },
            { label: "Products", value: String(stats.totalProducts), icon: Package, color: "text-indigo-400", bg: "bg-indigo-500/10" },
            { label: "Orders", value: String(stats.totalOrders), icon: ShoppingCart, color: "text-cyan-400", bg: "bg-cyan-500/10" },
            { label: "Customers", value: String(stats.totalCustomers), icon: Users, color: "text-amber-400", bg: "bg-amber-500/10" },
        ]
        : [];

    return (
        <div className="p-6 sm:p-8">
            <div className="mb-8">
                <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Overview</p>
                <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
            </div>

            {/* Stats Cards */}
            {loading ? (
                <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-10">
                    {[...Array(4)].map((_, i) => (
                        <div key={i} className="glass rounded-xl h-28 animate-pulse" />
                    ))}
                </div>
            ) : (
                <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-10">
                    {statCards.map((card) => (
                        <div key={card.label} className="glass rounded-xl p-5">
                            <div className="flex items-center justify-between mb-3">
                                <span className="text-xs font-medium text-zinc-500 uppercase tracking-wider">{card.label}</span>
                                <div className={`p-2 rounded-lg ${card.bg}`}>
                                    <card.icon className={`w-4 h-4 ${card.color}`} />
                                </div>
                            </div>
                            <span className="text-2xl font-bold tracking-tight">{card.value}</span>
                        </div>
                    ))}
                </div>
            )}

            {/* Recent Orders */}
            <div>
                <h2 className="text-lg font-semibold mb-4">Recent Orders</h2>
                <div className="glass rounded-xl overflow-hidden">
                    <div className="overflow-x-auto">
                        <table className="w-full text-sm">
                            <thead>
                                <tr className="border-b border-white/[0.06]">
                                    <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Order</th>
                                    <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Customer</th>
                                    <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Status</th>
                                    <th className="text-right px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                {loading
                                    ? [...Array(5)].map((_, i) => (
                                        <tr key={i} className="border-b border-white/[0.04]">
                                            <td colSpan={4} className="px-5 py-4"><div className="h-4 bg-white/[0.04] rounded animate-pulse" /></td>
                                        </tr>
                                    ))
                                    : recentOrders.map((order) => (
                                        <tr key={order.id} className="border-b border-white/[0.04] hover:bg-white/[0.02] transition">
                                            <td className="px-5 py-4 font-mono text-xs text-indigo-400">{order.orderNumber}</td>
                                            <td className="px-5 py-4 text-zinc-300">{order.customerName}</td>
                                            <td className="px-5 py-4">
                                                <span
                                                    className={`inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium ${order.status === "Delivered"
                                                            ? "bg-emerald-500/10 text-emerald-400"
                                                            : order.status === "Shipped"
                                                                ? "bg-cyan-500/10 text-cyan-400"
                                                                : order.status === "Cancelled"
                                                                    ? "bg-red-500/10 text-red-400"
                                                                    : "bg-amber-500/10 text-amber-400"
                                                        }`}
                                                >
                                                    {order.status}
                                                </span>
                                            </td>
                                            <td className="px-5 py-4 text-right font-medium">{formatCurrency(order.totalAmount)}</td>
                                        </tr>
                                    ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    );
}
