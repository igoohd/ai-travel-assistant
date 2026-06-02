"use client";

import { useState } from "react";
import type { TripRequestFormProps, TripRequestFormState } from "./types";
import { generateTrip } from "@/lib/api/tripsApi";
import type { GenerateTripResponse } from "@/lib/api/tripTypes";
import { toGenerateTripRequest, validateForm } from "./formHelpers";
import { TripResult } from "../trip-result";

export function TripRequestForm({ onTripGenerated }: TripRequestFormProps) {
  const [form, setForm] = useState<TripRequestFormState>({
    destination: "",
    numberOfDays: "5",
    budget: "3000",
    currency: "USD",
    interests: "",
  });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [generatedTrip, setGeneratedTrip] =
    useState<GenerateTripResponse | null>(null);

  function updateField(field: keyof TripRequestFormState, value: string) {
    setForm((currentForm) => ({
      ...currentForm,
      [field]: value,
    }));
  }

  async function handleSubmit(
    event: React.SyntheticEvent<HTMLFormElement, SubmitEvent>,
  ) {
    event.preventDefault();

    setErrorMessage(null);
    setGeneratedTrip(null);

    const validationErrors = validateForm(form);

    if (validationErrors.length > 0) {
      setErrorMessage(validationErrors.join(" "));
      return;
    }

    setIsSubmitting(true);

    try {
      const requestBody = toGenerateTripRequest(form);
      const trip = await generateTrip(requestBody);

      setGeneratedTrip(trip);
      onTripGenerated?.();
    } catch (error) {
      if (error instanceof Error) {
        setErrorMessage(error.message);
        return;
      }

      setErrorMessage("Something went wrong while generating the trip");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <section className="mt-8">
      <h2 className="text-lg font-semibold text-slate-950">Trip request</h2>
      <form className="mt-4 grid max-w-2xl gap-4" onSubmit={handleSubmit}>
        <label className="grid gap-1">
          <span className="text-sm font-medium text-slate-700">
            Destination
          </span>
          <input
            className="rounded-md border border-slate-300 px-3 py-2"
            value={form.destination}
            onChange={(event) => updateField("destination", event.target.value)}
          />
        </label>

        <label className="grid gap-1">
          <span className="text-sm font-medium text-slate-700">
            Number of days
          </span>
          <input
            className="rounded-md border border-slate-300 px-3 py-2"
            type="number"
            value={form.numberOfDays}
            onChange={(event) =>
              updateField("numberOfDays", event.target.value)
            }
          />
        </label>

        <label className="grid gap-1">
          <span className="text-sm font-medium text-slate-700">Budget</span>
          <input
            className="rounded-md border border-slate-300 px-3 py-2"
            type="number"
            value={form.budget}
            onChange={(event) => updateField("budget", event.target.value)}
          />
        </label>

        <label className="grid gap-1">
          <span className="text-sm font-medium text-slate-700">Currency</span>
          <input
            className="rounded-md border border-slate-300 px-3 py-2 uppercase"
            maxLength={3}
            value={form.currency}
            onChange={(event) => updateField("currency", event.target.value)}
          />
        </label>

        <label className="grid gap-1">
          <span className="text-sm font-medium text-slate-700">Interests</span>
          <input
            className="rounded-md border border-slate-300 px-3 py-2"
            placeholder="food, museums, technology"
            value={form.interests}
            onChange={(event) => updateField("interests", event.target.value)}
          />
        </label>

        <button
          className="w-fit rounded-md bg-slate-950 px-4 py-2 font-medium text-white"
          disabled={isSubmitting}
          type="submit"
        >
          {isSubmitting ? "Generating..." : "Generate trip"}
        </button>
      </form>
      {errorMessage ? (
        <p className="mt-4 rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
          {errorMessage}
        </p>
      ) : null}
      {generatedTrip ? <TripResult trip={generatedTrip} /> : null}
    </section>
  );
}
