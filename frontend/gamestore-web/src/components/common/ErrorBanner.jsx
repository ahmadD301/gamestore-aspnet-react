export default function ErrorBanner({
  message,
}) {
  return (
    <div
      role="alert"
      className="error-banner"
    >
      {message}
    </div>
  );
}