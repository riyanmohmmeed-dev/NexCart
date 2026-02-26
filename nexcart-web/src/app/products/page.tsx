"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { useSearchParams } from "next/navigation";
import { Search, SlidersHorizontal } from "lucide-react";
import { api } from "@/lib/api";
import { formatCurrency } from "@/lib/utils";
import type { Product, Category } from "@/lib/types";

export default function ProductsPage() {
    const searchParams = useSearchParams();
    const [products, setProducts] = useState<Product[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState("");
    const [categoryId, setCategoryId] = useState(searchParams.get("category") || "");
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        api.categories.list().then(setCategories).catch(() => { });
    }, []);

    useEffect(() => {
        setLoading(true);
        api.products
            .list({ page, pageSize: 12, search: search || undefined, categoryId: categoryId || undefined })
            .then((res) => {
                setProducts(res.items);
                setTotalPages(res.totalPages);
            })
            .catch(() => { })
            .finally(() => setLoading(false));
    }, [page, search, categoryId]);

    return (
        <div className="pt-16">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 py-10">
                {/* Header */}
                <div className="mb-8">
                    <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Store</p>
                    <h1 className="text-4xl font-bold tracking-tight">All Products</h1>
                </div>

                {/* Filters */}
                <div className="flex flex-col sm:flex-row gap-3 mb-8">
                    <div className="relative flex-1">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-500" />
                        <input
                            type="text"
                            placeholder="Search products..."
                            value={search}
                            onChange={(e) => { setSearch(e.target.value); setPage(1); }}
                            className="w-full pl-10 pr-4 py-2.5 rounded-xl bg-white/[0.04] border border-white/[0.08] text-sm text-white placeholder:text-zinc-500 focus:outline-none focus:border-indigo-500/50 focus:ring-1 focus:ring-indigo-500/20 transition"
                        />
                    </div>
                    <div className="relative">
                        <SlidersHorizontal className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-500" />
                        <select
                            value={categoryId}
                            onChange={(e) => { setCategoryId(e.target.value); setPage(1); }}
                            className="pl-10 pr-8 py-2.5 rounded-xl bg-white/[0.04] border border-white/[0.08] text-sm text-white focus:outline-none focus:border-indigo-500/50 appearance-none cursor-pointer min-w-[180px]"
                        >
                            <option value="">All Categories</option>
                            {categories.map((cat) => (
                                <option key={cat.id} value={cat.id} className="bg-zinc-900">{cat.name}</option>
                            ))}
                        </select>
                    </div>
                </div>

                {/* Grid */}
                {loading ? (
                    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                        {[...Array(8)].map((_, i) => (
                            <div key={i} className="glass rounded-xl h-48 animate-pulse" />
                        ))}
                    </div>
                ) : products.length === 0 ? (
                    <div className="text-center py-20">
                        <p className="text-zinc-500">No products found.</p>
                    </div>
                ) : (
                    <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
                        {products.map((product) => (
                            <Link
                                key={product.id}
                                href={`/products/${product.id}`}
                                className="glass rounded-xl p-5 group transition-all hover:-translate-y-1"
                            >
                                <div className="flex items-center justify-between mb-3">
                                    <span className="text-xs font-mono text-zinc-500">{product.categoryName}</span>
                                    {product.discountPercentage > 0 && (
                                        <span className="text-xs font-bold text-emerald-400 bg-emerald-500/10 px-2 py-0.5 rounded-full">
                                            -{product.discountPercentage}%
                                        </span>
                                    )}
                                </div>
                                <h3 className="font-semibold mb-1 group-hover:text-indigo-400 transition-colors line-clamp-2">
                                    {product.name}
                                </h3>
                                <p className="text-xs text-zinc-500 mb-4 line-clamp-2">{product.description}</p>
                                <div className="flex items-center justify-between mt-auto">
                                    <div className="flex items-baseline gap-2">
                                        <span className="text-lg font-bold">{formatCurrency(product.price)}</span>
                                        {product.compareAtPrice && (
                                            <span className="text-xs text-zinc-500 line-through">{formatCurrency(product.compareAtPrice)}</span>
                                        )}
                                    </div>
                                    {product.isLowStock && (
                                        <span className="text-xs text-amber-400">Low stock</span>
                                    )}
                                </div>
                                <div className="mt-3 flex items-center gap-1">
                                    {product.averageRating > 0 && (
                                        <span className="text-xs text-yellow-400">{"★".repeat(Math.round(product.averageRating))}</span>
                                    )}
                                    {product.totalReviews > 0 && (
                                        <span className="text-xs text-zinc-500">({product.totalReviews})</span>
                                    )}
                                </div>
                            </Link>
                        ))}
                    </div>
                )}

                {/* Pagination */}
                {totalPages > 1 && (
                    <div className="flex items-center justify-center gap-2 mt-10">
                        <button
                            disabled={page <= 1}
                            onClick={() => setPage(page - 1)}
                            className="px-4 py-2 rounded-lg text-sm font-medium bg-white/[0.04] border border-white/[0.08] text-zinc-400 hover:text-white disabled:opacity-30 disabled:cursor-not-allowed transition"
                        >
                            Previous
                        </button>
                        <span className="text-sm text-zinc-500 px-2">
                            {page} / {totalPages}
                        </span>
                        <button
                            disabled={page >= totalPages}
                            onClick={() => setPage(page + 1)}
                            className="px-4 py-2 rounded-lg text-sm font-medium bg-white/[0.04] border border-white/[0.08] text-zinc-400 hover:text-white disabled:opacity-30 disabled:cursor-not-allowed transition"
                        >
                            Next
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
}
