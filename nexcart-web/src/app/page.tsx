"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { ArrowRight, Sparkles, ShoppingBag, BarChart3, Shield } from "lucide-react";
import { api } from "@/lib/api";
import { formatCurrency } from "@/lib/utils";
import type { Product, Category } from "@/lib/types";

export default function HomePage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      api.products.list({ pageSize: 4, sortBy: "createdAt", sortDescending: true }),
      api.categories.list(),
    ])
      .then(([productsRes, cats]) => {
        setProducts(productsRes.items);
        setCategories(cats);
      })
      .catch(() => { })
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="pt-16">
      {/* ─── Hero ─── */}
      <section className="relative min-h-[80vh] flex items-center justify-center text-center px-4 overflow-hidden">
        {/* Glow */}
        <div className="absolute top-0 left-1/2 -translate-x-1/2 w-[800px] h-[500px] bg-gradient-radial from-indigo-500/15 via-indigo-500/5 to-transparent blur-3xl pointer-events-none" />

        <div className="relative z-10 max-w-3xl">
          <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-white/[0.04] border border-white/[0.08] text-sm text-zinc-400 mb-8">
            <Sparkles className="w-4 h-4 text-indigo-400" />
            Built with .NET Clean Architecture + Next.js
          </div>
          <h1 className="text-5xl sm:text-6xl lg:text-7xl font-extrabold tracking-tight leading-[1.1] mb-6">
            The store that
            <br />
            <span className="bg-gradient-to-r from-indigo-400 via-purple-400 to-cyan-400 bg-clip-text text-transparent">
              runs itself.
            </span>
          </h1>
          <p className="text-lg text-zinc-400 max-w-xl mx-auto mb-10 leading-relaxed">
            Enterprise e-commerce platform. Domain-Driven Design, CQRS pattern,
            MediatR pipeline — built for scale, designed for beauty.
          </p>
          <div className="flex items-center justify-center gap-3 flex-wrap">
            <Link
              href="/products"
              className="inline-flex items-center gap-2 px-6 py-3 rounded-xl bg-indigo-500 hover:bg-indigo-600 text-white font-semibold transition-all hover:shadow-lg hover:shadow-indigo-500/25"
            >
              <ShoppingBag className="w-4 h-4" />
              Browse Products
            </Link>
            <Link
              href="/admin"
              className="inline-flex items-center gap-2 px-6 py-3 rounded-xl bg-white/[0.04] border border-white/[0.08] text-zinc-300 hover:text-white hover:bg-white/[0.06] font-semibold transition-all"
            >
              <BarChart3 className="w-4 h-4" />
              Admin Dashboard
            </Link>
          </div>
        </div>
      </section>

      {/* ─── Architecture Features ─── */}
      <section className="px-4 sm:px-6 py-20 border-t border-white/[0.06]">
        <div className="max-w-6xl mx-auto">
          <div className="grid md:grid-cols-3 gap-4">
            {[
              {
                icon: Shield,
                title: "Clean Architecture",
                desc: "Domain → Application → Infrastructure → API. Zero leaky abstractions.",
              },
              {
                icon: Sparkles,
                title: "CQRS + MediatR",
                desc: "Every use case is a Command or Query. Pipeline behaviors for validation & logging.",
              },
              {
                icon: BarChart3,
                title: "Enterprise Ready",
                desc: "FluentValidation, Serilog structured logging, global exception handling, Scalar API docs.",
              },
            ].map((feat) => (
              <div
                key={feat.title}
                className="glass rounded-xl p-6 transition-all hover:-translate-y-1"
              >
                <feat.icon className="w-8 h-8 text-indigo-400 mb-4" />
                <h3 className="text-lg font-semibold mb-2">{feat.title}</h3>
                <p className="text-sm text-zinc-400 leading-relaxed">{feat.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* ─── Featured Products ─── */}
      <section className="px-4 sm:px-6 py-20">
        <div className="max-w-6xl mx-auto">
          <div className="flex items-center justify-between mb-8">
            <div>
              <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Featured</p>
              <h2 className="text-3xl font-bold tracking-tight">Latest Products</h2>
            </div>
            <Link
              href="/products"
              className="text-sm text-zinc-400 hover:text-indigo-400 transition flex items-center gap-1"
            >
              View all <ArrowRight className="w-4 h-4" />
            </Link>
          </div>

          {loading ? (
            <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-4">
              {[...Array(4)].map((_, i) => (
                <div key={i} className="glass rounded-xl h-64 animate-pulse" />
              ))}
            </div>
          ) : (
            <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-4">
              {products.map((product) => (
                <Link
                  key={product.id}
                  href={`/products/${product.id}`}
                  className="glass rounded-xl p-5 group transition-all hover:-translate-y-1"
                >
                  <div className="flex items-center justify-between mb-4">
                    <span className="text-xs font-mono text-zinc-500">{product.categoryName}</span>
                    {product.discountPercentage > 0 && (
                      <span className="text-xs font-bold text-emerald-400 bg-emerald-500/10 px-2 py-0.5 rounded-full">
                        -{product.discountPercentage}%
                      </span>
                    )}
                  </div>
                  <h3 className="font-semibold text-sm mb-2 group-hover:text-indigo-400 transition-colors line-clamp-2">
                    {product.name}
                  </h3>
                  <p className="text-xs text-zinc-500 mb-4 line-clamp-2">{product.description}</p>
                  <div className="flex items-baseline gap-2">
                    <span className="text-lg font-bold">{formatCurrency(product.price)}</span>
                    {product.compareAtPrice && (
                      <span className="text-xs text-zinc-500 line-through">
                        {formatCurrency(product.compareAtPrice)}
                      </span>
                    )}
                  </div>
                </Link>
              ))}
            </div>
          )}
        </div>
      </section>

      {/* ─── Categories ─── */}
      <section className="px-4 sm:px-6 py-20 border-t border-white/[0.06]">
        <div className="max-w-6xl mx-auto">
          <p className="text-xs font-mono text-indigo-400 uppercase tracking-widest mb-1">Browse by</p>
          <h2 className="text-3xl font-bold tracking-tight mb-8">Categories</h2>
          <div className="grid sm:grid-cols-2 lg:grid-cols-5 gap-3">
            {categories.map((cat) => (
              <Link
                key={cat.id}
                href={`/products?category=${cat.id}`}
                className="glass rounded-xl p-5 text-center group transition-all hover:-translate-y-1"
              >
                <h3 className="font-semibold text-sm mb-1 group-hover:text-indigo-400 transition-colors">
                  {cat.name}
                </h3>
                <p className="text-xs text-zinc-500">{cat.productCount} products</p>
              </Link>
            ))}
          </div>
        </div>
      </section>

      {/* ─── Footer ─── */}
      <footer className="px-4 sm:px-6 py-8 border-t border-white/[0.06] text-center">
        <p className="text-xs text-zinc-500 font-mono">
          NexCart © {new Date().getFullYear()} · Built with .NET 10 + Next.js · Mohammed Riyaan
        </p>
      </footer>
    </div>
  );
}
