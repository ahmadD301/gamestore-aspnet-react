import {
  test,
  expect,
}
from "@playwright/test";

const corsHeaders = {
  "access-control-allow-origin":
    "http://localhost:5173",
  "access-control-allow-credentials":
    "true",
};

test(
  "catalog page loads games",
  async ({ page }) => {
    const apiBaseUrl =
      "**://localhost:5062";

    await page.route(
      `${apiBaseUrl}/api/auth/refresh` ,
      (route) => {
        if (
          route.request().method()
          === "OPTIONS"
        ) {
          const allowHeaders =
            route.request().headers()[
              "access-control-request-headers"
            ];

          return route.fulfill({
            status: 204,
            headers: {
              ...corsHeaders,
              "access-control-allow-methods":
                "POST, OPTIONS",
              "access-control-allow-headers":
                allowHeaders || "content-type",
            },
          });
        }

        return route.fulfill({
          status: 401,
          headers: {
            ...corsHeaders,
            "content-type":
              "application/json",
          },
          body: JSON.stringify({
            message:
              "Unauthorized",
          }),
        });
      }
    );

    await page.route(
      `${apiBaseUrl}/api/games**`,
      (route) => {
        if (
          route.request().method()
          === "OPTIONS"
        ) {
          const allowHeaders =
            route.request().headers()[
              "access-control-request-headers"
            ];

          return route.fulfill({
            status: 204,
            headers: {
              ...corsHeaders,
              "access-control-allow-methods":
                "GET, OPTIONS",
              "access-control-allow-headers":
                allowHeaders || "content-type",
            },
          });
        }

        return route.fulfill({
          status: 200,
          headers: {
            ...corsHeaders,
            "content-type":
              "application/json",
          },
          body: JSON.stringify({
            items: [
              {
                id: 1,
                title:
                  "Skyline Racer",
                genre: "Action",
                description:
                  "High-speed races.",
                price: 19.99,
                releaseDate:
                  "2024-01-01",
              },
            ],
            page: 1,
            totalPages: 1,
          }),
        });
      }
    );

    await page.route(
      `${apiBaseUrl}/api/genres`,
      (route) => {
        if (
          route.request().method()
          === "OPTIONS"
        ) {
          const allowHeaders =
            route.request().headers()[
              "access-control-request-headers"
            ];

          return route.fulfill({
            status: 204,
            headers: {
              ...corsHeaders,
              "access-control-allow-methods":
                "GET, OPTIONS",
              "access-control-allow-headers":
                allowHeaders || "content-type",
            },
          });
        }

        return route.fulfill({
          status: 200,
          headers: {
            ...corsHeaders,
            "content-type":
              "application/json",
          },
          body: JSON.stringify([
            {
              id: 1,
              name: "Action",
            },
          ]),
        });
      }
    );

    await page.goto("/games");

    await expect(
      page.getByText(
        /game catalog/i
      )
    ).toBeVisible();

    await expect(
      page.locator(
        ".game-card"
      )
    ).toHaveCount(1);
  }
);