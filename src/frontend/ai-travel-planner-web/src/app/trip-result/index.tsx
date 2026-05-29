import type { TripResultProps } from "./types";

export function TripResult({ trip }: TripResultProps) {
  return (
    <section className="mt-6 rounded-md border border-slate-200 bg-white p-4">
      <h3 className="text-base font-semibold text-slate-950">
        {trip.destination}
      </h3>

      <p className="mt-1 text-sm text-slate-600">
        {trip.numberOfDays} days · {trip.budget.currency} {trip.budget.total}
      </p>

      <p className="mt-3 text-sm leading-6 text-slate-700">
        {trip.summary.overview}
      </p>

      <div className="mt-5 rounded-md border border-slate-200 bg-slate-50 p-4">
        <h4 className="text-sm font-semibold text-slate-950">
          Budget estimate
        </h4>

        <dl className="mt-3 grid gap-2 text-sm text-slate-600 sm:grid-cols-2">
          <div>
            <dt className="font-medium text-slate-900">Hotel</dt>
            <dd>
              {trip.budget.currency} {trip.budget.hotel}
            </dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Transportation</dt>
            <dd>
              {trip.budget.currency} {trip.budget.transportation}
            </dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Food</dt>
            <dd>
              {trip.budget.currency} {trip.budget.food}
            </dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Activities</dt>
            <dd>
              {trip.budget.currency} {trip.budget.activities}
            </dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Category</dt>
            <dd>{trip.budget.category}</dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Retries</dt>
            <dd>{trip.diagnostics.retryCount}</dd>
          </div>
        </dl>
      </div>

      {trip.validationIssues.length > 0 ? (
        <div className="mt-4 rounded-md border border-amber-200 bg-amber-50 p-4">
          <h4 className="text-sm font-semibold text-amber-950">
            Validation issues
          </h4>

          <ul className="mt-2 grid gap-2">
            {trip.validationIssues.map((issue) => (
              <li className="text-sm text-amber-800" key={issue.code}>
                <span className="font-medium">{issue.severity}</span>:{" "}
                {issue.message}
              </li>
            ))}
          </ul>
        </div>
      ) : null}

      <div className="mt-5 grid gap-4 sm:grid-cols-2">
        <div className="rounded-md border border-slate-200 bg-slate-50 p-4">
          <h4 className="text-sm font-semibold text-slate-950">Highlights</h4>

          <ul className="mt-2 grid gap-2">
            {trip.summary.highlights.map((highlight) => (
              <li className="text-sm leading-6 text-slate-600" key={highlight}>
                {highlight}
              </li>
            ))}
          </ul>
        </div>

        <div className="rounded-md border border-slate-200 bg-slate-50 p-4">
          <h4 className="text-sm font-semibold text-slate-950">Travel tips</h4>

          <ul className="mt-2 grid gap-2">
            {trip.summary.travelTips.map((tip) => (
              <li className="text-sm leading-6 text-slate-600" key={tip}>
                {tip}
              </li>
            ))}
          </ul>
        </div>
      </div>

      <div className="mt-5 grid gap-4">
        {trip.days.map((day) => (
          <article
            className="rounded-md border border-slate-200 bg-slate-50 p-4"
            key={day.dayNumber}
          >
            <h4 className="font-semibold text-slate-950">
              Day {day.dayNumber}: {day.title}
            </h4>

            <p className="mt-1 text-sm leading-6 text-slate-600">
              {day.description}
            </p>

            <div className="mt-4 grid gap-3">
              {day.activities.map((activity) => (
                <div
                  className="rounded-md border border-slate-200 bg-white p-3"
                  key={`${day.dayNumber}-${activity.timeOfDay}-${activity.title}`}
                >
                  <p className="text-xs font-medium uppercase text-slate-500">
                    {activity.timeOfDay}
                  </p>

                  <p className="mt-1 font-medium text-slate-900">
                    {activity.title}
                  </p>

                  <p className="mt-1 text-sm leading-6 text-slate-600">
                    {activity.description}
                  </p>
                </div>
              ))}
            </div>

            {day.restaurants.length > 0 ? (
              <div className="mt-4">
                <h5 className="text-sm font-semibold text-slate-900">
                  Restaurant ideas
                </h5>

                <ul className="mt-2 grid gap-2">
                  {day.restaurants.map((restaurant) => (
                    <li
                      className="text-sm text-slate-600"
                      key={`${day.dayNumber}-${restaurant.name}`}
                    >
                      <span className="font-medium text-slate-900">
                        {restaurant.name}
                      </span>{" "}
                      · {restaurant.cuisine} · {restaurant.notes}
                    </li>
                  ))}
                </ul>
              </div>
            ) : null}
          </article>
        ))}
      </div>
      <div className="mt-4 rounded-md border border-slate-200 bg-slate-50 p-4">
        <h4 className="text-sm font-semibold text-slate-950">AI metadata</h4>

        <dl className="mt-3 grid gap-2 text-sm text-slate-600 sm:grid-cols-2">
          <div>
            <dt className="font-medium text-slate-900">Provider</dt>
            <dd>{trip.aiMetadata.provider}</dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Model</dt>
            <dd>{trip.aiMetadata.model}</dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Prompt tokens</dt>
            <dd>{trip.aiMetadata.promptTokens ?? "Not available"}</dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Completion tokens</dt>
            <dd>{trip.aiMetadata.completionTokens ?? "Not available"}</dd>
          </div>

          <div>
            <dt className="font-medium text-slate-900">Total tokens</dt>
            <dd>{trip.aiMetadata.totalTokens ?? "Not available"}</dd>
          </div>
        </dl>
      </div>
    </section>
  );
}
