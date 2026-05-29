"use client";

import { useState } from "react";
import type { TripRequestFormState } from "./types";

export function TripRequestForm() {
  const [form, setForm] = useState<TripRequestFormState>({
    destination: "",
    numberOfDays: "5",
    budget: "3000",
    currency: "USD",
    interests: "",
  });

  function updateField(field: keyof TripRequestFormState, value: string) {
    setForm((currentForm) => ({
      ...currentForm,
      [field]: value,
    }));
  }

  return (
    <section className="mt-8">
      <h2 className="text-lg font-semibold text-slate-950">Trip request</h2>

      <form className="mt-4 grid max-w-2xl gap-4">
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
          type="submit"
        >
          Generate trip
        </button>
      </form>
    </section>
  );
}
