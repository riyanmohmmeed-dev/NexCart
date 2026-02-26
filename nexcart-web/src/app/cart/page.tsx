"use client";

import Link from "next/link";
import { Trash2, ShoppingBag, ArrowRight } from "lucide-react";
import { useCart } from "@/lib/store";
import { formatCurrency } from "@/lib/utils";

export default function CartPage() {
    const { items, updateQuantity, removeItem, clearCart, totalPrice } = useCart();

    return (
        <div className="pt-16">
            <div className="max-w-3xl mx-auto px-4 sm:px-6 py-10">
                <div className="mb-8">
                    <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Shopping</p>
                    <h1 className="text-4xl font-bold tracking-tight">Your Cart</h1>
                </div>

                {items.length === 0 ? (
                    <div className="text-center py-20">
                        <ShoppingBag className="w-12 h-12 text-zinc-600 mx-auto mb-4" />
                        <p className="text-zinc-500 mb-4">Your cart is empty.</p>
                        <Link
                            href="/products"
                            className="inline-flex items-center gap-2 text-sm text-indigo-400 hover:text-indigo-300 transition"
                        >
                            Browse products <ArrowRight className="w-4 h-4" />
                        </Link>
                    </div>
                ) : (
                    <>
                        <div className="space-y-3 mb-8">
                            {items.map((item) => (
                                <div
                                    key={item.product.id}
                                    className="glass rounded-xl p-4 flex items-center gap-4"
                                >
                                    <div className="flex-1 min-w-0">
                                        <Link
                                            href={`/products/${item.product.id}`}
                                            className="font-semibold text-sm hover:text-indigo-400 transition line-clamp-1"
                                        >
                                            {item.product.name}
                                        </Link>
                                        <p className="text-xs text-zinc-500">{item.product.categoryName}</p>
                                    </div>

                                    <div className="flex items-center rounded-lg bg-white/[0.04] border border-white/[0.08]">
                                        <button
                                            onClick={() => updateQuantity(item.product.id, item.quantity - 1)}
                                            className="px-3 py-1.5 text-zinc-400 hover:text-white text-sm transition"
                                        >
                                            −
                                        </button>
                                        <span className="px-2 text-sm font-medium min-w-[1.5rem] text-center">
                                            {item.quantity}
                                        </span>
                                        <button
                                            onClick={() => updateQuantity(item.product.id, item.quantity + 1)}
                                            className="px-3 py-1.5 text-zinc-400 hover:text-white text-sm transition"
                                        >
                                            +
                                        </button>
                                    </div>

                                    <span className="text-sm font-bold w-24 text-right">
                                        {formatCurrency(item.product.price * item.quantity)}
                                    </span>

                                    <button
                                        onClick={() => removeItem(item.product.id)}
                                        className="p-2 text-zinc-500 hover:text-red-400 transition"
                                    >
                                        <Trash2 className="w-4 h-4" />
                                    </button>
                                </div>
                            ))}
                        </div>

                        {/* Summary */}
                        <div className="glass rounded-xl p-6">
                            <div className="flex items-center justify-between mb-4">
                                <span className="text-sm text-zinc-400">Subtotal</span>
                                <span className="text-lg font-bold">{formatCurrency(totalPrice())}</span>
                            </div>
                            <div className="flex items-center justify-between mb-4 pb-4 border-b border-white/[0.06]">
                                <span className="text-sm text-zinc-400">Shipping</span>
                                <span className="text-sm text-zinc-400">
                                    {totalPrice() >= 100 ? (
                                        <span className="text-emerald-400">Free</span>
                                    ) : (
                                        formatCurrency(9.99)
                                    )}
                                </span>
                            </div>
                            <div className="flex items-center justify-between mb-6">
                                <span className="font-semibold">Total</span>
                                <span className="text-2xl font-bold">
                                    {formatCurrency(totalPrice() + (totalPrice() >= 100 ? 0 : 9.99))}
                                </span>
                            </div>
                            <div className="flex gap-3">
                                <button
                                    onClick={clearCart}
                                    className="px-4 py-2.5 rounded-xl text-sm font-medium bg-white/[0.04] border border-white/[0.08] text-zinc-400 hover:text-white transition"
                                >
                                    Clear Cart
                                </button>
                                <button className="flex-1 px-6 py-2.5 rounded-xl bg-indigo-500 hover:bg-indigo-600 text-white font-semibold transition-all hover:shadow-lg hover:shadow-indigo-500/25 text-sm">
                                    Checkout
                                </button>
                            </div>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}
