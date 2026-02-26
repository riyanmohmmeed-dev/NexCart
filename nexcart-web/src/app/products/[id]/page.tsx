"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { ArrowLeft, ShoppingCart, Star, Package, AlertTriangle } from "lucide-react";
import { api } from "@/lib/api";
import { useCart } from "@/lib/store";
import { formatCurrency } from "@/lib/utils";
import type { Product } from "@/lib/types";

export default function ProductDetailPage() {
    const { id } = useParams<{ id: string }>();
    const router = useRouter();
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [quantity, setQuantity] = useState(1);
    const [added, setAdded] = useState(false);
    const addItem = useCart((s) => s.addItem);

    useEffect(() => {
        if (!id) return;
        api.products
            .get(id)
            .then(setProduct)
            .catch(() => { })
            .finally(() => setLoading(false));
    }, [id]);

    const handleAddToCart = () => {
        if (!product) return;
        addItem(product, quantity);
        setAdded(true);
        setTimeout(() => setAdded(false), 2000);
    };

    if (loading) {
        return (
            <div className="pt-16 max-w-4xl mx-auto px-4 sm:px-6 py-10">
                <div className="glass rounded-2xl h-96 animate-pulse" />
            </div>
        );
    }

    if (!product) {
        return (
            <div className="pt-16 max-w-4xl mx-auto px-4 sm:px-6 py-20 text-center">
                <p className="text-zinc-500">Product not found.</p>
            </div>
        );
    }

    return (
        <div className="pt-16 max-w-4xl mx-auto px-4 sm:px-6 py-10">
            <button
                onClick={() => router.back()}
                className="inline-flex items-center gap-2 text-sm text-zinc-400 hover:text-white transition mb-8"
            >
                <ArrowLeft className="w-4 h-4" /> Back
            </button>

            <div className="glass rounded-2xl p-6 sm:p-8">
                <div className="grid md:grid-cols-2 gap-8">
                    {/* Left: Product Image Placeholder */}
                    <div className="flex items-center justify-center bg-gradient-to-br from-indigo-500/10 to-purple-500/10 rounded-xl min-h-[300px] border border-white/[0.04]">
                        <Package className="w-20 h-20 text-indigo-400/40" />
                    </div>

                    {/* Right: Product Info */}
                    <div className="flex flex-col">
                        <div className="flex items-center gap-2 mb-2">
                            <span className="text-xs font-mono text-indigo-400 uppercase tracking-wider">
                                {product.categoryName}
                            </span>
                            <span className="text-xs text-zinc-600">·</span>
                            <span className="text-xs font-mono text-zinc-500">{product.sku}</span>
                        </div>

                        <h1 className="text-2xl sm:text-3xl font-bold tracking-tight mb-4">
                            {product.name}
                        </h1>

                        <p className="text-zinc-400 text-sm leading-relaxed mb-6">
                            {product.description}
                        </p>

                        {/* Price */}
                        <div className="flex items-baseline gap-3 mb-6">
                            <span className="text-3xl font-bold">{formatCurrency(product.price)}</span>
                            {product.compareAtPrice && (
                                <>
                                    <span className="text-lg text-zinc-500 line-through">
                                        {formatCurrency(product.compareAtPrice)}
                                    </span>
                                    <span className="text-sm font-bold text-emerald-400 bg-emerald-500/10 px-2 py-0.5 rounded-full">
                                        Save {product.discountPercentage}%
                                    </span>
                                </>
                            )}
                        </div>

                        {/* Rating */}
                        {product.averageRating > 0 && (
                            <div className="flex items-center gap-2 mb-6">
                                <div className="flex">
                                    {[...Array(5)].map((_, i) => (
                                        <Star
                                            key={i}
                                            className={`w-4 h-4 ${i < Math.round(product.averageRating) ? "text-yellow-400 fill-yellow-400" : "text-zinc-600"}`}
                                        />
                                    ))}
                                </div>
                                <span className="text-sm text-zinc-400">
                                    {product.averageRating.toFixed(1)} ({product.totalReviews} reviews)
                                </span>
                            </div>
                        )}

                        {/* Stock */}
                        <div className="flex items-center gap-2 mb-8">
                            {product.stockQuantity > 0 ? (
                                <>
                                    <span className={`w-2 h-2 rounded-full ${product.isLowStock ? "bg-amber-400" : "bg-emerald-400"}`} />
                                    <span className="text-sm text-zinc-400">
                                        {product.isLowStock ? (
                                            <span className="text-amber-400">Only {product.stockQuantity} left</span>
                                        ) : (
                                            "In Stock"
                                        )}
                                    </span>
                                </>
                            ) : (
                                <>
                                    <AlertTriangle className="w-4 h-4 text-red-400" />
                                    <span className="text-sm text-red-400">Out of Stock</span>
                                </>
                            )}
                        </div>

                        {/* Add to Cart */}
                        <div className="flex items-center gap-3 mt-auto">
                            <div className="flex items-center rounded-xl bg-white/[0.04] border border-white/[0.08]">
                                <button
                                    onClick={() => setQuantity(Math.max(1, quantity - 1))}
                                    className="px-4 py-2.5 text-zinc-400 hover:text-white transition"
                                >
                                    −
                                </button>
                                <span className="px-3 text-sm font-medium min-w-[2rem] text-center">{quantity}</span>
                                <button
                                    onClick={() => setQuantity(Math.min(product.stockQuantity, quantity + 1))}
                                    className="px-4 py-2.5 text-zinc-400 hover:text-white transition"
                                >
                                    +
                                </button>
                            </div>
                            <button
                                onClick={handleAddToCart}
                                disabled={product.stockQuantity <= 0}
                                className="flex-1 inline-flex items-center justify-center gap-2 px-6 py-3 rounded-xl bg-indigo-500 hover:bg-indigo-600 text-white font-semibold transition-all disabled:opacity-40 disabled:cursor-not-allowed hover:shadow-lg hover:shadow-indigo-500/25"
                            >
                                <ShoppingCart className="w-4 h-4" />
                                {added ? "Added!" : "Add to Cart"}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
