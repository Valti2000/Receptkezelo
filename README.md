TASK:

1. Create an application capable of managing a recipe collection. Recipes have a code name, a title (dish name), description, and ingredients. Ingredients within a recipe can be grouped into categories (e.g., ingredients for meat, ingredients for side dishes or sauces, etc.). Each ingredient has a raw material, quantity, and unit of measurement. Raw materials belong to different categories, with each raw material belonging to only one category. One raw material can be associated with multiple ingredients.

2. Allergens can be associated with raw materials, and one allergen can belong to multiple raw materials, while one raw material can have multiple allergens.

3. Based on points 1 and 2, create entities and their dependencies in the application. Create the database using Entity Framework Core 6 (EF) migration. Use EF services for all database operations.

4. The application should allow listing all recipes, ingredients, ingredient categories, and allergens separately through different endpoints.

5. Retrieve a complete recipe, including ingredients, description, and any allergens.

6. Retrieve all ingredients belonging to a specific category and all ingredients associated with a specific allergen.

7. All entities should be manageable in the system, allowing creation, modification, retrieval, and logical deletion (CRUD operations). Implement necessary endpoints for associating data with each other.

8. Set up a global query filter so that by default, only non-deleted entities are retrieved from the database. Allow specifying whether deleted entities should be included in all listing endpoints.

9. Enhance recipe data with preparation time. Preparation time is the sum of preparation and cooking times. Modify the endpoints for creating and modifying recipes to include this information. The specification of preparation time should be optional, and if the user does not provide any value, the field should be set to 0. Store only the cooking and preparation times in the database for each recipe, but make the information accessible through the recipe entity in the software.

10. Allow querying which recipes contain a specific ingredient. Retrieve recipes from the database sorted by cooking time. Include the recipe name, allergens derived from its ingredients, and all other data in the list, including deleted recipes.

11. When deleting, check whether the entity to be deleted is a dependency of another entity that is not deleted. If it is a dependency, prevent deletion and return an error code and message.

12. Ensure that every endpoint returns the appropriate HTTP code and, if necessary, a message.

13. Create the Repository and UnitOfWork layers. Refactor the application so that database operations go through these layers.

14. Expand the system using IdentityUser to allow user registration, login, and logout. Users should have a name, residence (city and country), and a profile picture (URL).

15. Users should be able to mark recipes as favorites and undo this marking. If a recipe is marked as a favorite by a user, it can still be deleted as an entity.

16. Utilize authentication and authorization middleware. Generate an authentication JWT token and use it for subsequent operations.

17. Create roles in the system using IdentityRole and assign them to users. Roles should include Admin (with full access), Recipe Writer (able to add new recipes), and Recipe Reader (can view recipes and manage favorites). Store user roles in their claims.

18. Develop middleware that assigns a unique identifier (Guid) to the request and formats the response accordingly.

19. If there is no user with the Admin role in the database when the program starts, automatically create a default user with that role.

20. Create an endpoint for registering a new user, automatically assigning the Recipe Reader role.

21. Create an endpoint for a user with the Recipe Reader role to mark allergens they wish to avoid. Later, recipes containing these allergens should be flagged when the user queries them.

22. Implement authorization for endpoints using Authorize and AllowAnonymous attributes:
    a. Registration, login, allergen, and recipe listing should be accessible to anyone.
    b. Other queries (GET endpoints) should only be accessible to registered users.
    c. Data input, modification, deletion, and association endpoints should only be accessible to users with the Admin role.
    d. Recipe creation and editing endpoints should also be accessible to users with the Recipe Writer role.
    e. Create a new endpoint that lists ingredients based on allergens in the incoming request. Only users with Recipe Writer or Recipe Reader roles should be able to access this endpoint.
    f. Create a policy that filters requests, allowing only active (not deleted or blocked) users to retrieve the data specified in point e.

23. Based on the Request logging topic, log incoming requests to the console/file, but only those requiring authorization. Include at least the following data in the log:
    a. HTTP method of the request
    b. Name of the request endpoint
    c. Request body in JSON or XML format
    d. Identifier of the user initiating the request
    e. Name of the user initiating the request

24. Cache allergens and ingredients in memory. Modify services to use the cache for allergens and ingredients, ensuring consistency with the database at all times.

25. Use and configure Swagger in the application to generate API documentation.

26. Use model validation attributes on the properties of your models. Apply these attributes sensibly to your properties.

27. Create a custom model validation attribute for recipe codes. The code should consist of at least 6 and at most 12 characters, only containing numbers or capital letters, not starting with a number, not containing spaces, and only including underscore (_) and exclamation mark (!) among special characters.

28. Optional: Organize the database connection configuration in the appsettings.json file. Use the Options pattern implementation in the application for configuration usage.
