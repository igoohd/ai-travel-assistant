"use client";

import { useEffect, useState } from "react";
import { getTrip, listTrips, validateTrip } from "@/lib/api/tripsApi";
import type {
  GenerateTripResponse,
  TripListItemResponse,
} from "@/lib/api/tripTypes";
import { TripResult } from "../trip-result";

export function RecentTrips() {
  const [trips, setTrips] = useState<TripListItemResponse[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [selectedTrip, setSelectedTrip] = useState<GenerateTripResponse | null>(
    null,
  );
  const [isLoadingTrip, setIsLoadingTrip] = useState(false);
  const [validationMessagesByTripId, setValidationMessagesByTripId] = useState<
    Record<string, string[]>
  >({});
  const [validatingTripId, setValidatingTripId] = useState<string | null>(null);
  const [hasLoadedTrips, setHasLoadedTrips] = useState(false);
  const [isRefreshingTrips, setIsRefreshingTrips] = useState(false);

  async function refreshTrips() {
    setIsRefreshingTrips(true);

    try {
      const response = await listTrips();

      setErrorMessage(null);
      setTrips(response.trips);
    } catch (error) {
      if (error instanceof Error) {
        setErrorMessage(error.message);
        return;
      }

      setErrorMessage("Something went wrong while loading trips.");
    } finally {
      setIsRefreshingTrips(false);
      setHasLoadedTrips(true);
    }
  }

  useEffect(() => {
    listTrips()
      .then((response) => {
        setErrorMessage(null);
        setTrips(response.trips);
      })
      .catch((error: unknown) => {
        if (error instanceof Error) {
          setErrorMessage(error.message);
          return;
        }

        setErrorMessage("Something went wrong while loading trips.");
      })
      .finally(() => {
        setHasLoadedTrips(true);
      });
  }, []);

  async function handleOpenTrip(id: string) {
    if (selectedTrip?.id === id) {
      setSelectedTrip(null);
      return;
    }

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

  async function handleValidateTrip(id: string) {
    setValidatingTripId(id);
    setValidationMessagesByTripId((current) => ({
      ...current,
      [id]: [],
    }));
    setErrorMessage(null);

    try {
      const response = await validateTrip(id);

      setValidationMessagesByTripId((current) => ({
        ...current,
        [id]: response.validationIssues.map(
          (issue) => `${issue.severity}: ${issue.message}`,
        ),
      }));
    } catch (error) {
      if (error instanceof Error) {
        setErrorMessage(error.message);
        return;
      }

      setErrorMessage("Something went wrong while validating the trip.");
    } finally {
      setValidatingTripId(null);
    }
  }
  return (
    <section className="mt-10">
      <h2 className="text-lg font-semibold text-slate-950 mb-2">
        Recent trips
      </h2>

      <button
        className="rounded-md border border-slate-300 px-3 py-1.5 text-sm font-medium text-slate-700 mb-3"
        disabled={isRefreshingTrips}
        type="button"
        onClick={() => void refreshTrips()}
      >
        {isRefreshingTrips ? "Refreshing..." : "Refresh"}
      </button>

      {errorMessage ? (
        <p className="mt-3 rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
          {errorMessage}
        </p>
      ) : null}

      <div className="mt-4 grid gap-3">
        {!hasLoadedTrips ? (
          <p className="mt-4 rounded-md border border-slate-200 bg-white p-4 text-sm text-slate-600">
            Loading trips...
          </p>
        ) : null}

        {hasLoadedTrips && trips.length === 0 ? (
          <p className="mt-4 rounded-md border border-slate-200 bg-white p-4 text-sm text-slate-600">
            No trips generated yet.
          </p>
        ) : null}
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
              className="mt-3 mr-3 rounded-md border border-slate-300 px-3 py-1.5 text-sm font-medium text-slate-700"
              disabled={isLoadingTrip}
              type="button"
              onClick={() => handleOpenTrip(trip.id)}
            >
              {selectedTrip?.id === trip.id ? "Close" : "Open"}
            </button>
            <button
              className="mt-3 rounded-md border border-slate-300 px-3 py-1.5 text-sm font-medium text-slate-700"
              disabled={validatingTripId === trip.id}
              type="button"
              onClick={() => handleValidateTrip(trip.id)}
            >
              {validatingTripId === trip.id ? "Validating..." : "Validate"}
            </button>
            {selectedTrip?.id === trip.id ? (
              <div className="mt-4">
                <TripResult trip={selectedTrip} />

                {(validationMessagesByTripId[trip.id] ?? []).length > 0 ? (
                  <ul className="mt-4 grid gap-2">
                    {(validationMessagesByTripId[trip.id] ?? []).map(
                      (message) => (
                        <li className="text-sm text-amber-800" key={message}>
                          {message}
                        </li>
                      ),
                    )}
                  </ul>
                ) : null}
              </div>
            ) : null}
          </article>
        ))}
      </div>
    </section>
  );
}
