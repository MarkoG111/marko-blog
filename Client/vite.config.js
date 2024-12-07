import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

const proxyPaths = [
  '/posts',
  '/categories',
  '/notifications',
  '/auth',
  '/users',
  '/comments',
  '/images',
  '/followers',
  '/authorrequests',
  '/usecaselogs',
  '/register',
  '/login',
  '/notificationsHub'
]

const createProxyConfig = (path) => ({
  target: 'http://localhost:5207',
  secure: false,
  ws: path === '/notificationsHub', // Enable WebSocket proxy for notificationsHub only
})

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    proxy: Object.fromEntries(proxyPaths.map((path) => [path, createProxyConfig(path)])),
  },
  plugins: [react()],
})
