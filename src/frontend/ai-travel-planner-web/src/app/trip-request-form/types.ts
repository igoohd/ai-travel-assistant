export type TripRequestFormState = {
  destination: string;
  numberOfDays: string;
  budget: string;
  currency: string;
  interests: string;
};

export type TripRequestFormProps = {
  onTripGenerated?: () => void;
};
