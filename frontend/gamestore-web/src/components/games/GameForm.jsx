import { useState } from "react";

export default function GameForm({
  initialValues,
  genres,
  onSubmit,
  isLoading,
}) {
  const [formData, setFormData] =
    useState(
      initialValues ?? {
        title: "",
        description: "",
        price: "",
        releaseDate: "",
        genreId: "",
      }
    );

  const [errors, setErrors] =
    useState({});

  function validate() {
    const newErrors = {};

    if (!formData.title.trim()) {
      newErrors.title =
        "Title is required.";
    }

    if (
      formData.description.length < 10
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

    if (!validate()) {
      return;
    }

    onSubmit({
      ...formData,
      price:
        Number(formData.price),
    });
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

  return (
    <form
      onSubmit={handleSubmit}
      className="game-form"
    >
      <div>
        <label>Title</label>

        <input
          type="text"
          value={formData.title}
          onChange={(e) =>
            updateField(
              "title",
              e.target.value
            )
          }
        />

        {errors.title && (
          <span>{errors.title}</span>
        )}
      </div>

      <div>
        <label>Description</label>

        <textarea
          value={
            formData.description
          }
          onChange={(e) =>
            updateField(
              "description",
              e.target.value
            )
          }
        />

        {errors.description && (
          <span>
            {errors.description}
          </span>
        )}
      </div>

      <div>
        <label>Price</label>

        <input
          type="number"
          step="0.01"
          value={formData.price}
          onChange={(e) =>
            updateField(
              "price",
              e.target.value
            )
          }
        />

        {errors.price && (
          <span>{errors.price}</span>
        )}
      </div>

      <div>
        <label>Release Date</label>

        <input
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

      <div>
        <label>Genre</label>

        <select
          value={formData.genreId}
          onChange={(e) =>
            updateField(
              "genreId",
              e.target.value
            )
          }
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

      <button
        type="submit"
        disabled={isLoading}
      >
        {isLoading
          ? "Saving..."
          : "Save Game"}
      </button>
    </form>
  );
}