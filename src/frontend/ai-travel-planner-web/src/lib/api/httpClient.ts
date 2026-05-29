import { ApiError, readErrorResponse } from "./apiError";

const apiBaseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;

export async function request<TResponse>(
  path: string,
  init?: RequestInit,
): Promise<TResponse> {
  if (!apiBaseUrl) {
    throw new Error("NEXT_PUBLIC_API_BASE_URL is not configured.");
  }

  const headers = new Headers(init?.headers);

  if (init?.body) {
    headers.set("Content-Type", "application/json");
  }

  const response = await fetch(`${apiBaseUrl}${path}`, {
    ...init,
    cache: "no-store",
    headers,
  });

  if (!response.ok) {
    const errorResponse = await readErrorResponse(response);

    throw new ApiError(
      errorResponse.errors.join(" "),
      response.status,
      errorResponse.errors,
    );
  }

  return response.json() as Promise<TResponse>;
}