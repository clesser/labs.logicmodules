import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    watch: {
      usePolling: true,
      interval: 300,
      ignored: (watchedPath: string): boolean => {
        return watchedPath.includes("\\.vs\\")
          || watchedPath.includes("/.vs/");
      }
    }
  }
});
