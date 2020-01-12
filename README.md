# customer-api-exercise
Technical exercise demonstrating .Net Core skills

Technologies used:
 - ASP.NET Core 2.2
 - .NET standard 2.0 (class libraries)
 - Swagger/OpenApi
 - xUnit unit test framework
 - Moq mocking framework
 - EntityFramework in memory datastore

Notes:
 - I have separated the business and data objects and map between the two as needed. This is because some datastores have a lot of metadata that is useful in the data layer but not for the business layer and client.
 - The input and output models for the Api are also different, we don't to have to specify an Id for a POST operation. For PUT operations, the Id is already given.