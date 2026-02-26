"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { LayoutDashboard, Package, ShoppingCart, ArrowLeft } from "lucide-react";
import { cn } from "@/lib/utils";

const adminLinks = [
    { href: "/admin", label: "Dashboard", icon: LayoutDashboard },
    { href: "/admin/products", label: "Products", icon: Package },
    { href: "/admin/orders", label: "Orders", icon: ShoppingCart },
];

export default function AdminLayout({ children }: { children: React.ReactNode }) {
    const pathname = usePathname();

    return (
        <div className="pt-16 flex min-h-screen">
            {/* Sidebar */}
            <aside className="w-56 border-r border-white/[0.06] bg-zinc-950/50 flex flex-col p-4 hidden md:flex">
                <div className="mb-6">
                    <Link href="/" className="inline-flex items-center gap-2 text-xs text-zinc-500 hover:text-zinc-300 transition">
                        <ArrowLeft className="w-3 h-3" /> Back to Store
                    </Link>
                </div>
                <p className="text-[10px] font-mono text-zinc-600 uppercase tracking-widest mb-3 px-3">Admin</p>
                <nav className="flex flex-col gap-1">
                    {adminLinks.map((link) => (
                        <Link
                            key={link.href}
                            href={link.href}
                            className={cn(
                                "flex items-center gap-2.5 px-3 py-2 rounded-lg text-sm font-medium transition-colors",
                                pathname === link.href
                                    ? "text-white bg-white/[0.06]"
                                    : "text-zinc-500 hover:text-white hover:bg-white/[0.04]"
                            )}
                        >
                            <link.icon className="w-4 h-4" />
                            {link.label}
                        </Link>
                    ))}
                </nav>
            </aside>

            {/* Content */}
            <div className="flex-1 overflow-auto">
                {/* Mobile admin nav */}
                <div className="md:hidden flex gap-1 p-3 border-b border-white/[0.06] overflow-x-auto">
                    {adminLinks.map((link) => (
                        <Link
                            key={link.href}
                            href={link.href}
                            className={cn(
                                "flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium whitespace-nowrap transition",
                                pathname === link.href
                                    ? "text-white bg-white/[0.06]"
                                    : "text-zinc-400"
                            )}
                        >
                            <link.icon className="w-3.5 h-3.5" />
                            {link.label}
                        </Link>
                    ))}
                </div>
                {children}
            </div>
        </div>
    );
}
