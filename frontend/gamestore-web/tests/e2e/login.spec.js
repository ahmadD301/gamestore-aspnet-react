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
  "admin login works",
  async ({ page }) => {
    await page.route(
      "**/api/auth/refresh",
      (route) => {
        if (
          route.request().method()
          === "OPTIONS"
        ) {
          return route.fulfill({
            status: 204,
            headers: {
              ...corsHeaders,
              "access-control-allow-methods":
                "POST, OPTIONS",
              "access-control-allow-headers":
                "content-type",
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
      "**/api/auth/login",
      (route) => {
        if (
          route.request().method()
          === "OPTIONS"
        ) {
          return route.fulfill({
            status: 204,
            headers: {
              ...corsHeaders,
              "access-control-allow-methods":
                "POST, OPTIONS",
              "access-control-allow-headers":
                "content-type",
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
            accessToken:
              "test-token",
            email:
              "admin@gamestore.com",
            userName:
              "Admin",
            roles: ["Admin"],
          }),
        });
      }
    );

    await page.goto("/login");

    await page
      .getByLabel(/email/i)
      .fill(
        "admin@gamestore.com"
      );

    await page
      .getByLabel(/password/i)
      .fill("Admin123!");

    await page
      .getByRole("button", {
        name: /login/i,
      })
      .click();

    await expect(page)
      .toHaveURL("/admin");

    await expect(
      page.getByRole("heading", {
        name: /admin dashboard/i,
      })
    ).toBeVisible({ timeout: 10000 });
  }
);