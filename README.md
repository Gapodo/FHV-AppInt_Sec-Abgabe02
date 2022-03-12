# Assignment 02 - shopsystem

## People

* Marion Hölzl
* Michael Amann

## Assumptions

* we are not targeting RMM Level 3, while nice it's not common practice / common standard
* An article cannot be moved to another shop (i.e. the ShopId or Shop reference is immutable)
* An article is only editable via it's respective shop (the shop always needs to be provided)
* Deleting the shop cascades through to the articles (deleting the shop deletes it's contents)
* Article prices may be negative to allow for bonuses (i.e. deposit-returns)

## Using

All commands are to be executed in the project root (the directory this README.md file is located in).

Spin up the demo container via docker-compose `docker-compose up -d` or run your own database and change the connection-string in the `apsettings.Development.json`.

Apply the Migrations using `dotnet ef database update` and start using by running `dotnet run`.

The API is structured as `api/shops` `api/shops/{shopId}` `api/shops/{shopId}/articles` `api/shops/{shopId}/articles/{articleId}`.

Articles are only available within their store (shopId must match).

## Hacking

To hack use `dotnet watch run .` to allow for live refreshes, a preliminary swaggerUi is provided at the `swagger/` endpoint.