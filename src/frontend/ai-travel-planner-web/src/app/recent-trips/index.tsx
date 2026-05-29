"use client";

import { useEffect, useState } from "react";
import { getTrip, listTrips } from "@/lib/api/tripsApi";
import type {
  GenerateTripResponse,
  TripListItemResponse,
} from "@/lib/api/tripTypes";

export function RecentTrips() {
  const [trips, setTrips] = useState<TripListItemResponse[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [selectedTrip, setSelectedTrip] = useState<GenerateTripResponse | null>(
    null,
  );
  const [isLoadingTrip, setIsLoadingTrip] = useState(false);

  useEffect(() => {
    async function loadTrips() {
      try {
        const response = await listTrips();
        setTrips(response.trips);
      } catch (error) {
        if (error instanceof Error) {
          setErrorMessage(error.message);
          return;
        }

        setErrorMessage("Something went wrong while loading trips.");
      }
    }

    loadTrips();
  }, []);

  async function handleOpenTrip(id: string) {
    setIsLoadingTrip(true);
    setErrorMessage(null);

    try {
      const trip = await getTrip(id);
      setSelectedTrip(trip);
    } catch (error) {
      if (error instanceof Error) {
        setErrorMessage(error.message);
        return;
      }

      setErrorMessage("Something went wrong while loading the trip.");
    } finally {
      setIsLoadingTrip(false);
    }
  }
  return (
    <section className="mt-10">
      <h2 className="text-lg font-semibold text-slate-950">Recent trips</h2>

      {errorMessage ? (
        <p className="mt-3 rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
          {errorMessage}
        </p>
      ) : null}

      <div className="mt-4 grid gap-3">
        {trips.map((trip) => (
          <article
            className="rounded-md border border-slate-200 bg-white p-4"
            key={trip.id}
          >
            <h3 className="font-medium text-slate-950">{trip.destination}</h3>

            <p className="mt-1 text-sm text-slate-600">
              {trip.numberOfDays} days · {trip.currency} {trip.estimatedTotal} ·{" "}
              {trip.budgetCategory}
            </p>

            <button
              className="mt-3 rounded-md border border-slate-300 px-3 py-1.5 text-sm font-medium text-slate-700"
              disabled={isLoadingTrip}
              type="button"
              onClick={() => handleOpenTrip(trip.id)}
            >
              Open
            </button>
            {selectedTrip?.id === trip.id ? (
              <div className="mt-4 rounded-md border border-slate-200 bg-slate-50 p-4">
                <p className="text-sm leading-6 text-slate-700">
                  {selectedTrip.summary.overview}
                </p>

                <div className="mt-4 grid gap-3">
                  {selectedTrip.days.map((day) => (
                    <article
                      className="rounded-md border border-slate-200 bg-white p-3"
                      key={day.dayNumber}
                    >
                      <h4 className="font-medium text-slate-950">
                        Day {day.dayNumber}: {day.title}
                      </h4>

                      <p className="mt-1 text-sm leading-6 text-slate-600">
                        {day.description}
                      </p>
                    </article>
                  ))}
                </div>
              </div>
            ) : null}
          </article>
        ))}
      </div>
    </section>
  );
}
