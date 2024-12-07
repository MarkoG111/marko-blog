import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    proxy: {
      '/posts': {
        target: 'http://localhost:5207',
        secure: false,
      },
      '/categories': {
        target: 'http://localhost:5207',
        secure: false,
      },
      '/notifications': {
        target: 'http://localhost:5207',
        secure: false,
      },
      '/notificationsHub': {
        target: 'http://localhost:5207',
        secure: false,
        ws: true, // Enable WebSocket proxying
      },
    },
  },

  plugins: [react()],
})
