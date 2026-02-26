"use client";

import { useEffect, useState } from "react";
import { Plus, Pencil, Trash2 } from "lucide-react";
import { api } from "@/lib/api";
import { formatCurrency } from "@/lib/utils";
import type { Product } from "@/lib/types";

export default function AdminProductsPage() {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const loadProducts = () => {
        setLoading(true);
        api.products
            .list({ page, pageSize: 10 })
            .then((res) => {
                setProducts(res.items);
                setTotalPages(res.totalPages);
            })
            .catch(() => { })
            .finally(() => setLoading(false));
    };

    useEffect(() => { loadProducts(); }, [page]);

    const handleDelete = async (id: string) => {
        if (!confirm("Deactivate this product?")) return;
        try {
            await api.products.delete(id);
            loadProducts();
        } catch { }
    };

    return (
        <div className="p-6 sm:p-8">
            <div className="flex items-center justify-between mb-8">
                <div>
                    <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Manage</p>
                    <h1 className="text-3xl font-bold tracking-tight">Products</h1>
                </div>
                <button className="inline-flex items-center gap-2 px-4 py-2 rounded-xl bg-indigo-500 hover:bg-indigo-600 text-white text-sm font-semibold transition hover:shadow-lg hover:shadow-indigo-500/25">
                    <Plus className="w-4 h-4" /> Add Product
                </button>
            </div>

            <div className="glass rounded-xl overflow-hidden">
                <div className="overflow-x-auto">
                    <table className="w-full text-sm">
                        <thead>
                            <tr className="border-b border-white/[0.06]">
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Product</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Category</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Price</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Stock</th>
                                <th className="text-left px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Status</th>
                                <th className="text-right px-5 py-3 text-xs font-medium text-zinc-500 uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading
                                ? [...Array(5)].map((_, i) => (
                                    <tr key={i} className="border-b border-white/[0.04]">
                                        <td colSpan={6} className="px-5 py-4"><div className="h-4 bg-white/[0.04] rounded animate-pulse" /></td>
                                    </tr>
                                ))
                                : products.map((p) => (
                                    <tr key={p.id} className="border-b border-white/[0.04] hover:bg-white/[0.02] transition">
                                        <td className="px-5 py-4">
                                            <div>
                                                <p className="font-medium text-zinc-200 line-clamp-1">{p.name}</p>
                                                <p className="text-xs text-zinc-500 font-mono">{p.sku}</p>
                                            </div>
                                        </td>
                                        <td className="px-5 py-4 text-zinc-400">{p.categoryName}</td>
                                        <td className="px-5 py-4">
                                            <span className="font-medium">{formatCurrency(p.price)}</span>
                                            {p.compareAtPrice && (
                                                <span className="text-xs text-zinc-500 line-through ml-2">{formatCurrency(p.compareAtPrice)}</span>
                                            )}
                                        </td>
                                        <td className="px-5 py-4">
                                            <span className={p.isLowStock ? "text-amber-400" : p.stockQuantity <= 0 ? "text-red-400" : "text-zinc-400"}>
                                                {p.stockQuantity}
                                            </span>
                                        </td>
                                        <td className="px-5 py-4">
                                            <span className={`inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium ${p.isActive ? "bg-emerald-500/10 text-emerald-400" : "bg-red-500/10 text-red-400"}`}>
                                                {p.isActive ? "Active" : "Inactive"}
                                            </span>
                                        </td>
                                        <td className="px-5 py-4 text-right">
                                            <div className="flex items-center justify-end gap-1">
                                                <button className="p-1.5 rounded-lg text-zinc-500 hover:text-indigo-400 hover:bg-white/[0.04] transition">
                                                    <Pencil className="w-3.5 h-3.5" />
                                                </button>
                                                <button
                                                    onClick={() => handleDelete(p.id)}
                                                    className="p-1.5 rounded-lg text-zinc-500 hover:text-red-400 hover:bg-white/[0.04] transition"
                                                >
                                                    <Trash2 className="w-3.5 h-3.5" />
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* Pagination */}
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
