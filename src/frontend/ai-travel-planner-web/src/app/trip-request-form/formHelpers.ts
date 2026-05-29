import type { GenerateTripRequest } from "@/lib/api/tripTypes";
import type { TripRequestFormState } from "./types";

export function toGenerateTripRequest(
  form: TripRequestFormState,
): GenerateTripRequest {
  return {
    destination: form.destination.trim(),
    numberOfDays: parseInt(form.numberOfDays, 10),
    budget: parseFloat(form.budget),
    currency: form.currency.trim().toUpperCase(),
    interests: form.interests
      .split(",")
      .map((interest) => interest.trim())
      .filter((interest) => interest.length > 0),
  };
}

export function validateForm(form: TripRequestFormState): string[] {
  const errors: string[] = [];

  if (form.destination.trim().length < 2) {
    errors.push("Destination must have at least 2 characters.");
  }

  if (Number.isNaN(parseInt(form.numberOfDays, 10))) {
    errors.push("Number of days is required.");
  }

  if (parseInt(form.numberOfDays, 10) < 1) {
    errors.push("Number of days must be at least 1.");
  }

  if (parseInt(form.numberOfDays, 10) > 30) {
    errors.push("Number of days must be 30 or fewer.");
  }

  if (Number.isNaN(parseFloat(form.budget))) {
    errors.push("Budget is required.");
  }

  if (parseFloat(form.budget) < 1) {
    errors.push("Budget must be at least 1.");
  }

  if (form.currency.trim().length !== 3) {
    errors.push("Currency must be a 3-letter code.");
  }

  if (toGenerateTripRequest(form).interests.length === 0) {
    errors.push("At least one interest is required.");
  }

  return errors;
}
