# Assignment 02 - shopsystem

## People

* Marion Hölzl
* Michael Amann

## Using

Spin up the demo container via docker-compose `docker-compose up -d` or run your own database and change the connection-string in the `apsettings.Development.json`.

Apply the Migrations using `dotnet ef database update`.

## Assumptions

* we are not targeting RMM Level 3, while nice it's not common practice / common standard
* An article cannot be moved to another shop (i.e. the ShopId or Shop reference is immutable)
* An article is only editable via it's respective shop (the shop always needs to be provided)
* Deleting the shop cascades through to the articles (deleting the shop deletes it's contents)