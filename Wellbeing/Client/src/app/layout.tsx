import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Wellbeing App",
  description: "Wellbeing application",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
