export type ApiErrorResponse = {
  errors: string[];
};

export class ApiError extends Error {
  constructor(
    message: string,
    public readonly status: number,
    public readonly errors: string[],
  ) {
    super(message);
    this.name = "ApiError";
  }
}

export async function readErrorResponse(response: Response): Promise<ApiErrorResponse> {
  try {
    const body = (await response.json()) as Partial<ApiErrorResponse>;

    if (Array.isArray(body.errors)) {
      return { errors: body.errors };
    }
  } catch {
    // Fall through to a generic error below.
  }

  return { errors: [`Request failed with status ${response.status}.`] };
}