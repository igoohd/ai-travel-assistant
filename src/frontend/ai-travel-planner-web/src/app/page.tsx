export default function Home() {
  return (
    <main className="min-h-screen px-6 py-10">
      <section className="mx-auto flex w-full max-w-5xl flex-col gap-3">
        <p className="text-sm font-medium text-slate-500">AI Travel Planner</p>
        <h1 className="text-3xl font-semibold text-slate-950">
          Plan a trip with AI assistance
        </h1>
        <p className="max-w-2xl text-base leading-7 text-slate-600">
          This screen is intentionally clean. Next we will connect it to the API
          and build the trip request flow step by step.
        </p>
      </section>
    </main>
  );
}
