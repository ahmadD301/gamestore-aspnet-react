export default function EmptyState({
  title,
  description,
}) {
  return (
    <section
      className="empty-state"
      aria-labelledby="empty-title"
    >
      <h2 id="empty-title">
        {title}
      </h2>

      <p>{description}</p>
    </section>
  );
}