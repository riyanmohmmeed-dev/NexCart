"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useState } from "react";
import { ShoppingCart, Menu, X, Package } from "lucide-react";
import { useCart } from "@/lib/store";
import { cn } from "@/lib/utils";

const links = [
    { href: "/", label: "Home" },
    { href: "/products", label: "Products" },
    { href: "/cart", label: "Cart" },
    { href: "/admin", label: "Admin" },
];

export function Navbar() {
    const pathname = usePathname();
    const [open, setOpen] = useState(false);
    const totalItems = useCart((s) => s.totalItems());
    const isAdmin = pathname.startsWith("/admin");

    return (
        <header
            className={cn(
                "fixed top-0 left-0 right-0 z-50 h-16 border-b transition-all duration-300",
                "bg-zinc-950/80 backdrop-blur-xl border-white/[0.06]"
            )}
        >
            <nav className="max-w-7xl mx-auto px-4 sm:px-6 h-full flex items-center justify-between">
                {/* Brand */}
                <Link href="/" className="flex items-center gap-2 group">
                    <div className="flex items-center justify-center w-8 h-8 rounded-lg bg-indigo-500/10 border border-indigo-500/20 group-hover:bg-indigo-500/20 transition">
                        <Package className="w-4 h-4 text-indigo-400" />
                    </div>
                    <span className="font-bold text-lg tracking-tight">
                        Nex<span className="text-indigo-400">Cart</span>
                    </span>
                </Link>

                {/* Desktop Nav */}
                <div className="hidden md:flex items-center gap-1">
                    {links.map((link) => (
                        <Link
                            key={link.href}
                            href={link.href}
                            className={cn(
                                "px-3 py-2 rounded-lg text-sm font-medium transition-colors",
                                pathname === link.href || (link.href !== "/" && pathname.startsWith(link.href))
                                    ? "text-white bg-white/[0.06]"
                                    : "text-zinc-400 hover:text-white hover:bg-white/[0.04]"
                            )}
                        >
                            {link.label}
                        </Link>
                    ))}
                </div>

                {/* Cart + Mobile Toggle */}
                <div className="flex items-center gap-2">
                    {!isAdmin && (
                        <Link
                            href="/cart"
                            className="relative flex items-center justify-center w-10 h-10 rounded-lg text-zinc-400 hover:text-white hover:bg-white/[0.04] transition"
                        >
                            <ShoppingCart className="w-5 h-5" />
                            {totalItems > 0 && (
                                <span className="absolute -top-1 -right-1 w-5 h-5 rounded-full bg-indigo-500 text-[11px] font-bold flex items-center justify-center text-white">
                                    {totalItems}
                                </span>
                            )}
                        </Link>
                    )}
                    <button
                        onClick={() => setOpen(!open)}
                        className="md:hidden flex items-center justify-center w-10 h-10 rounded-lg text-zinc-400 hover:text-white transition"
                    >
                        {open ? <X className="w-5 h-5" /> : <Menu className="w-5 h-5" />}
                    </button>
                </div>
            </nav>

            {/* Mobile Menu */}
            {open && (
                <div className="md:hidden bg-zinc-950/98 backdrop-blur-xl border-b border-white/[0.06] px-4 py-3">
                    {links.map((link) => (
                        <Link
                            key={link.href}
                            href={link.href}
                            onClick={() => setOpen(false)}
                            className={cn(
                                "block px-3 py-2 rounded-lg text-sm font-medium transition-colors mb-1",
                                pathname === link.href
                                    ? "text-white bg-white/[0.06]"
                                    : "text-zinc-400 hover:text-white"
                            )}
                        >
                            {link.label}
                        </Link>
                    ))}
                </div>
            )}
        </header>
    );
}
