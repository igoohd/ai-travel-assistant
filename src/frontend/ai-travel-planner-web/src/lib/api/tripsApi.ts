import { request } from "./httpClient";
import type {
  GenerateTripRequest,
  GenerateTripResponse,
  ListTripsResponse,
  ValidateTripResponse,
} from "./tripTypes";

export function generateTrip(
  requestBody: GenerateTripRequest,
): Promise<GenerateTripResponse> {
  return request<GenerateTripResponse>("/api/trips/generate", {
    method: "POST",
    body: JSON.stringify(requestBody),
  });
}

export function listTrips(): Promise<ListTripsResponse> {
  return request<ListTripsResponse>("/api/trips");
}

export function getTrip(id: string): Promise<GenerateTripResponse> {
  return request<GenerateTripResponse>(`/api/trips/${id}`);
}

export function validateTrip(id: string): Promise<ValidateTripResponse> {
  return request<ValidateTripResponse>(`/api/trips/${id}/validate`, {
    method: "POST",
  });
}
