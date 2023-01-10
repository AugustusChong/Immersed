import axios from "axios";
import {
  API_HOST_PREFIX,
  onGlobalError,
  onGlobalSuccess,
} from "./serviceHelpers";

const endpoint = `${API_HOST_PREFIX}/api/organizations`;

const getTotalUsers = () => {
  const config = {
    method: "GET",
    url: `${endpoint}/totalUsers`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getTotalTrainees = () => {
  const config = {
    method: "GET",
    url: `${endpoint}/totalTrainees`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const organizationsService = {
  getTotalUsers,
  getTotalTrainees,
};

export default organizationsService;
