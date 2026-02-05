import Link from 'next/link';

export default function Home() {
  return (
    <main className="flex min-h-screen flex-col items-center justify-center p-24">
      <div className="z-10 max-w-5xl w-full items-center justify-between font-mono text-sm">
        <h1 className="text-4xl font-bold text-center mb-8">
          Welcome to Wellbeing App
        </h1>
        <p className="text-center text-gray-600 mb-8">
          Get started by editing{" "}
          <code className="font-mono font-bold">src/app/page.tsx</code>
        </p>
        <div className="text-center">
          <Link
            href="/survey/1"
            className="inline-block bg-purple-600 hover:bg-purple-700 text-white font-semibold py-3 px-8 rounded-lg transition-colors duration-200"
          >
            View Survey Landing Page (Example)
          </Link>
        </div>
      </div>
    </main>
  );
}
