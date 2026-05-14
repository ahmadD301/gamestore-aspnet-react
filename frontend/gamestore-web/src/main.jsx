import React from "react";
import ReactDOM from "react-dom/client";

import { QueryClient } from "@tanstack/react-query";
import { QueryClientProvider } from "@tanstack/react-query";

import App from "./App";
import { AuthProvider } from "./auth/AuthContext";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import "./index.css";
  
const queryClient = new QueryClient();

ReactDOM.createRoot(
  document.getElementById("root")
).render(
  <React.StrictMode>
    <QueryClientProvider
      client={queryClient}
    >
      <AuthProvider>
        <App />
        <ReactQueryDevtools initialIsOpen={false} />
      </AuthProvider>
    </QueryClientProvider>

  </React.StrictMode>
);