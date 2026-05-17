export default function ErrorBanner({
  message,
}) {
  return (
    <div
      className="error-banner"
      role="alert"
      aria-live="assertive"
    >
      {message}
    </div>
  );
}