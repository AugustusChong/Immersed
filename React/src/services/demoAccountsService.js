import axios from "axios";
import {
  API_HOST_PREFIX,
  onGlobalError,
  onGlobalSuccess,
} from "./serviceHelpers";
const _logger = debug.extend("demoAccountsService");

const endpoint = `${API_HOST_PREFIX}/api/demoaccounts`;

const getActive = () => {
  _logger("getActive is running");
  const config = {
    method: "GET",
    url: `${endpoint}/active`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const demoAccountsService = {
  getActive,
};

export default demoAccountsService;
