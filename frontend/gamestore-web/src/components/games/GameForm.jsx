import { useEffect, useState } from "react";

export default function GameForm({
  initialValues,
  genres,
  onSubmit,
  onCancel,
  isLoading,
}) {
  const defaultValues = {
    title: "",
    description: "",
    price: "",
    coverImageUrl: "",
    releaseDate: "",
    genreId: "",
  };

  const [formData, setFormData] =
    useState(
      {
        ...defaultValues,
        ...(initialValues ?? {}),
      }
    );

  const [errors, setErrors] =
    useState({});

  const [imagePreview, setImagePreview] =
    useState("");

  useEffect(() => {
    if (formData.coverImageUrl) {
      setImagePreview(formData.coverImageUrl);
    }

    return () => {
      if (imagePreview?.startsWith("blob:")) {
        URL.revokeObjectURL(imagePreview);
      }
    };
  }, [formData.coverImageUrl, imagePreview]);

  function validate() {
    const newErrors = {};

    if (!formData.title?.trim()) {
      newErrors.title =
        "Title is required.";
    }

    if (
      (formData.description ?? "")
        .length < 10
    ) {
      newErrors.description =
        "Description too short.";
    }

    if (
      Number(formData.price) < 0
    ) {
      newErrors.price =
        "Price cannot be negative.";
    }

    if (!formData.genreId) {
      newErrors.genreId =
        "Genre is required.";
    }

    setErrors(newErrors);

    return (
      Object.keys(newErrors).length === 0
    );
  }

  function handleSubmit(event) {
    event.preventDefault();

    const action =
      event.nativeEvent?.submitter?.dataset
        ?.action || "save";

    if (!validate()) {
      return;
    }

    onSubmit(
      {
        title: formData.title,
        description:
          formData.description,
        price:
          Number(formData.price),
        coverImageUrl:
          formData.coverImageUrl,
        releaseDateUtc:
          formData.releaseDate || null,
        genreId: formData.genreId,
      },
      action
    );
  }

  function updateField(
    field,
    value
  ) {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  }

  function handleImageChange(event) {
    const file = event.target.files?.[0];

    if (!file) {
      setImagePreview("");
      updateField("coverImageUrl", "");
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      const result =
        typeof reader.result === "string"
          ? reader.result
          : "";
      updateField("coverImageUrl", result);
      setImagePreview(result);
    };
    reader.readAsDataURL(file);
  }

  return (
    <form
      onSubmit={handleSubmit}
      className="game-form"
    >
      <div className="form-grid">
        <div className="form-field">
          <label htmlFor="title">
            Title
            <span className="required" aria-hidden="true">*</span>
          </label>

          <input
            id="title"
            type="text"
            value={formData.title}
            onChange={(e) =>
              updateField(
                "title",
                e.target.value
              )
            }
            aria-invalid={!!errors.title}
            aria-describedby={
              errors.title
                ? "title-error"
                : undefined
            }
            required
          />

          {errors.title && (
            <span
              id="title-error"
              role="alert"
            >
              {errors.title}
            </span>
          )}
        </div>

        <div className="form-field">
          <label htmlFor="price">
            Price
          </label>

          <input
            id="price"
            type="number"
            step="0.01"
            value={formData.price}
            onChange={(e) =>
              updateField(
                "price",
                e.target.value
              )
            }
            aria-invalid={!!errors.price}
          />

          {errors.price && (
            <span>{errors.price}</span>
          )}
        </div>

        <div className="form-field full">
          <label htmlFor="description">
            Description
            <span className="required" aria-hidden="true">*</span>
          </label>

          <textarea
            id="description"
            value={
              formData.description
            }
            onChange={(e) =>
              updateField(
                "description",
                e.target.value
              )
            }
            aria-invalid={!!errors.description}
            required
          />

          {errors.description && (
            <span>
              {errors.description}
            </span>
          )}
        </div>

        <div className="form-field">
          <label htmlFor="release-date">Release Date</label>

          <input
            id="release-date"
            type="date"
            value={
              formData.releaseDate
            }
            onChange={(e) =>
              updateField(
                "releaseDate",
                e.target.value
              )
            }
          />
        </div>

        <div className="form-field">
          <label htmlFor="genre">
            Genre
            <span className="required" aria-hidden="true">*</span>
          </label>

          <select
            id="genre"
            value={formData.genreId}
            onChange={(e) =>
              updateField(
                "genreId",
                e.target.value
              )
            }
            aria-invalid={!!errors.genreId}
            required
          >
            <option value="">
              Select Genre
            </option>

            {genres.map((genre) => (
              <option
                key={genre.id}
                value={genre.id}
              >
                {genre.name}
              </option>
            ))}
          </select>

          {errors.genreId && (
            <span>
              {errors.genreId}
            </span>
          )}
        </div>

        <div className="form-field full">
          <label htmlFor="coverImageUrl">Cover Image URL</label>
          <input
            id="coverImageUrl"
            type="url"
            placeholder="https://example.com/cover.png"
            value={formData.coverImageUrl}
            onChange={(e) =>
              updateField("coverImageUrl", e.target.value)
            }
          />
        </div>

        <div className="form-field full">
          <label htmlFor="image">Or upload image</label>
          <div className="image-upload">
            <div className="image-preview">
              {imagePreview ? (
                <img src={imagePreview} alt="Game cover preview" />
              ) : (
                <span className="muted">No image selected</span>
              )}
            </div>
            <input
              id="image"
              type="file"
              accept="image/*"
              onChange={handleImageChange}
            />
          </div>
        </div>
      </div>

      <div className="form-actions">
        <button
          type="button"
          className="button secondary"
          onClick={onCancel}
        >
          Cancel
        </button>
        <button
          type="submit"
          className="button secondary"
          data-action="continue"
          disabled={isLoading}
        >
          Save & Continue
        </button>
        <button
          type="submit"
          className="button primary"
          data-action="save"
          disabled={isLoading}
        >
          {isLoading
            ? "Saving..."
            : "Save Game"}
        </button>
      </div>
    </form>
  );
}