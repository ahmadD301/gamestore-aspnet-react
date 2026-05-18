import {
  render,
  screen,
}
from "@testing-library/react";

import GameCard
from "../../src/components/games/GameCard";

// eslint-disable-next-line no-undef
describe("GameCard", () => {
  // eslint-disable-next-line no-undef
  it(
    "renders game data",
    () => {
      render(
        <GameCard
          game={{
            id: "1",
            title: "Cyberpunk 2077",
            genre: "RPG",
            price: 59.99,
          }}
        />
      );

      // eslint-disable-next-line no-undef
      expect(
        screen.getByText(
          "Cyberpunk 2077"
        )
      ).toBeInTheDocument();

      // eslint-disable-next-line no-undef
      expect(
        screen.getByText("RPG")
      ).toBeInTheDocument();
    }
  );
});