import { lazy } from "react";

const InternalAnalytics = lazy(() =>
  import("../components/dashboard/analytics/internal/InternalAnalytics")
);

const dashboardRoutes = [
  {
    path: "/dashboard",
    name: "Dashboards",
    icon: "uil-home-alt",
    header: "Navigation",
    children: [
      {
        path: "/dashboard/analytics/internal",
        name: "AnalyticsInternal",
        element: InternalAnalytics,
        roles: ["SysAdmin"],
        exact: true,
        isAnonymous: false,
      },
    ],
  },
];

const securedRoutes = [...dashboardRoutes];

export default securedRoutes;
