// Axios package
import axios from "axios";

// Stablish base url and create connection
const api = axios.create({
  baseURL: "https://localhost:7097/api",
});

export default api;
