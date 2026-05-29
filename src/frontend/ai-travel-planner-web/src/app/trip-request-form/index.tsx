"use client";

import { useState } from "react";
import type { TripRequestFormState } from "./types";
import { generateTrip } from "@/lib/api/tripsApi";
import { GenerateTripResponse } from "@/lib/api/tripTypes";

export function TripRequestForm() {
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
    useState<GenerateTripResponse | null>();

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

    setIsSubmitting(true);
    setErrorMessage(null);
    setGeneratedTrip(null);

    try {
      const trip = await generateTrip({
        destination: form.destination,
        numberOfDays: parseInt(form.numberOfDays, 10),
        budget: parseFloat(form.budget),
        currency: form.currency,
        interests: form.interests
          .split(",")
          .map((interest) => interest.trim())
          .filter((interest) => interest.length > 0),
      });

      setGeneratedTrip(trip);
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
      {generatedTrip ? (
        <section className="mt-6 rounded-md border border-slate-200 bg-white p-4">
          <h3 className="text-base font-semibold text-slate-950">
            {generatedTrip.destination}
          </h3>
          <p className="mt-1 text-sm text-slate-600">
            {generatedTrip.numberOfDays} days · {generatedTrip.budget.currency}{" "}
            {generatedTrip.budget.total}
          </p>
          <p className="mt-3 text-sm leading-6 text-slate-700">
            {generatedTrip.summary.overview}
          </p>
        </section>
      ) : null}
    </section>
  );
}
