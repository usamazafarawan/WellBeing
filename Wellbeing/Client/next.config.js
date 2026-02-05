/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  // Disable certificate validation for localhost in development
  // This is needed because the API server uses self-signed certificates
  async rewrites() {
    return [];
  },
}

// For Node.js server-side fetch, we need to disable certificate validation for localhost
if (process.env.NODE_ENV === 'development') {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

module.exports = nextConfig
