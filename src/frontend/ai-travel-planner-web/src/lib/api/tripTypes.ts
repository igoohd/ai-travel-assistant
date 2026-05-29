export type GenerateTripRequest = {
  destination: string;
  numberOfDays: number;
  budget: number;
  interests: string[];
  currency: string;
};

export type GenerateTripResponse = {
  id: string;
  destination: string;
  numberOfDays: number;
  days: DayResponse[];
  budget: BudgetEstimateResponse;
  summary: SummaryResponse;
  validationIssues: ValidationIssueResponse[];
  aiMetadata: AiGenerationResponse;
  diagnostics: GenerationDiagnosticsResponse;
};

export type DayResponse = {
  dayNumber: number;
  title: string;
  description: string;
  activities: ActivityResponse[];
  restaurants: RestaurantSuggestionResponse[];
};

export type ActivityResponse = {
  timeOfDay: string;
  title: string;
  description: string;
  estimatedCost: number;
  durationHours: number;
  transitMinutesFromPrevious: number;
};

export type RestaurantSuggestionResponse = {
  name: string;
  cuisine: string;
  notes: string;
  estimatedCost: number;
};

export type BudgetEstimateResponse = {
  hotel: number;
  transportation: number;
  food: number;
  activities: number;
  total: number;
  category: string;
  currency: string;
};

export type SummaryResponse = {
  overview: string;
  highlights: string[];
  travelTips: string[];
};

export type ValidationIssueResponse = {
  code: string;
  message: string;
  severity: string;
};

export type AiGenerationResponse = {
  provider: string;
  model: string;
  promptTokens: number | null;
  completionTokens: number | null;
  totalTokens: number | null;
};

export type GenerationDiagnosticsResponse = {
  retryCount: number;
};

export type ListTripsResponse = {
  trips: TripListItemResponse[];
};

export type TripListItemResponse = {
  id: string;
  createdAt: string;
  destination: string;
  numberOfDays: number;
  estimatedTotal: number;
  currency: string;
  budgetCategory: string;
  aiMetadata: AiGenerationResponse;
};

export type ValidateTripResponse = {
  validationIssues: ValidationIssueResponse[];
};
