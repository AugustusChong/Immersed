import axios from "axios";
import * as helper from "./serviceHelpers";
const _logger = debug.extend("userService");

const endpoint = `${helper.API_HOST_PREFIX}/api/users`;

const getStatusTotals = () => {
  _logger("getStatusTotals running");
  const config = {
    method: "GET",
    url: `${endpoint}/status/totals`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getStatusOverTime = () => {
  _logger("getStatusOverTime running");
  const config = {
    method: "GET",
    url: `${endpoint}/status/overTime`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const userService = {
  getStatusTotals,
  getStatusOverTime,
};

export default userService;
