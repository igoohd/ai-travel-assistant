"use client";

import { useState } from "react";
import type { Tab } from "./types";
import { TripRequestForm } from "../trip-request-form";
import { RecentTrips } from "../recent-trips";

export function Tabs() {
  const [recentTripsRefreshKey, setRecentTripsRefreshKey] = useState(0);

  const tabs: Tab[] = [
    {
      id: "generate",
      label: "Generate Trip",
      content: (
        <TripRequestForm
          onTripGenerated={() =>
            setRecentTripsRefreshKey((currentKey) => currentKey + 1)
          }
        />
      ),
    },
    {
      id: "recent",
      label: "Recent Trips",
      content: <RecentTrips refreshKey={recentTripsRefreshKey} />,
    },
  ];
  const [selectedTabId, setSelectedTabId] = useState(tabs[0].id);

  const selectedTab = tabs.find((tab) => tab.id === selectedTabId) ?? tabs[0];

  return (
    <div className="mt-8">
      <div className="flex gap-2 border-b border-slate-200">
        {tabs.map((tab) => (
          <button
            className={
              tab.id === selectedTabId
                ? "border-b-2 border-slate-950 px-3 py-2 text-sm font-medium text-slate-950"
                : "px-3 py-2 text-sm font-medium text-slate-500"
            }
            key={tab.id}
            type="button"
            onClick={() => setSelectedTabId(tab.id)}
          >
            {tab.label}
          </button>
        ))}
      </div>

      <div>{selectedTab.content}</div>
    </div>
  );
}
