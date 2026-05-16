import axios from "axios";

export const apiClient = axios.create({
  baseURL: "http://localhost:5062",
  withCredentials: true,
});