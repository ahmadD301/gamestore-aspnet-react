export default function EmptyState({
  title,
  description,
  actionLabel,
  onAction,
}) {
  return (
    <section
      className="empty-state"
      aria-labelledby="empty-title"
    >
      <div className="empty-icon" aria-hidden="true" />
      <h2 id="empty-title">
        {title}
      </h2>

      <p>{description}</p>

      {actionLabel && onAction && (
        <button
          type="button"
          className="button secondary"
          onClick={onAction}
        >
          {actionLabel}
        </button>
      )}
    </section>
  );
}