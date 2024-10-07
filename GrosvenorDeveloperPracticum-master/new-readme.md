# .NET Developer Practicum - Refactoring

**What Was Done**

Package Fixes: Fixed missing packages in the project that were preventing the console app from running. After the installation, the console app ran without any issues.

Project Updates: All projects were updated to .NET 8.

Instruction Logs: Created instruction logs for the user, so they know the format required for submitting requests.

Flow Adjustments: Some methods were breaking when called. For example:

ParseOrder: It was not prepared to handle a string at the beginning of the order list (morning, 1, 2).
Implementation of New Features: New features were implemented as requested.

Creation of Web.App:

It is now possible to make the same requests that the console app does.
It uses an in-memory database that is created and populated when the project runs.
New methods were added that essentially perform the same functions as the old methods, but now query the database. This allows for the creation and listing of new items.
Test Project Updates: The new methods were implemented in the test project. An instance that initializes the in-memory database was created, allowing tests to be performed both for mocked data and database queries.

**Documentation**

When running Web.App, you will have access to the project's Swagger documentation.

#Endpoints with /mock are simply adaptations of the methods from the console app, with the same mocked values.

[GET] https://localhost:7261/api/DishManager/mock/morningmenu

Response:
{
    "Mornin: | 1 - egg | 2 -  toast | 3 - coffee |"
}

[GET] https://localhost:7261/api/DishManager/mock/eveningmenu

Response:
{
    "Evening: | 1 - steak | 2 - potato  | 3 - wine  | 4 - cake |"
}

[GET] https://localhost:7261/api/DishManager/seeMenu

Response:
{
  "morning": [
    {
      "id": 5,
      "dishName": "egg"
    },
    {
      "id": 6,
      "dishName": "toast"
    },
    {
      "id": 7,
      "dishName": "coffee"
    }
  ],
  "evening": [
    {
      "id": 1,
      "dishName": "steak"
    },
    {
      "id": 2,
      "dishName": "potato"
    },
    {
      "id": 3,
      "dishName": "wine"
    },
    {
      "id": 4,
      "dishName": "cake"
    }
  ]
}

[POST] https://localhost:7261/api/DishManager/addDish

Request body example:

{
  "time": "morning",
  "dishName": "Milk",
  "count": 1
}

[POST] https://localhost:7261/api/Server/mock/takeorder

Request body example:

{
    "Morning, 1, 2"
}

[POST] https://localhost:7261/api/Server/takeOrder

Request body example:

{
    "morning, 5" //5 == DishId
}