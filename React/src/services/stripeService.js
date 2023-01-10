import axios from "axios";
import {
  API_HOST_PREFIX,
  onGlobalError,
  onGlobalSuccess,
} from "./serviceHelpers";

const endpoint = `${API_HOST_PREFIX}/api/stripe`;

const getTotalRevenue = () => {
  const config = {
    method: "GET",
    url: `${endpoint}/revenue`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const stripeService = {
  getTotalRevenue,
};
export default stripeService;
