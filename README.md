# OrderDetailsAPI

Requirement is to build a REST API to supply order details for a ecommerce website..

I have created a REST service for fetching Customer's recent order details using .net Core, Entity Framework Core, Microsoft SQL as database and used Entity framework migration nuget package for generating entities from DB using `Scaffold-DbContext` Command. 
Added Swagger for consumers to easily integrate with their application.

## How to run
### DB Connection
Make sure you have valid SQL connection string under ConnectionStrings setting of *appsettings.json*

### Endpoint and test details
- There is swagger implementation for the API to know the Request and Response with status code in details. Available in Json and can also access after running the application.
- The local url for swagger - https://localhost:44349/swagger/index.html
There is 1 endpoints as mentioned below,
- `POST` /api/OrderDetails
#### Request
NOTE : Please use valid customer details. For security reason I have provided mock details
`{
{
  "user": "test@test.com",
  "customerId": "T12345"
}`

**Using CURL**

`curl -X POST "https://localhost:44349/api/OrderDetails" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"user\":\"test@test.com\",\"customerId\":\"T12345\"}"`
#### Response
`{
  "customer": {
    "firstName": "First",
    "lastName": "Last"
  },
  "order": {
    "orderNumber": 4,
    "orderDate": "2020-09-11T00:00:00",
    "deliveryAddress": "1a, Uppingham Gate, Uppingham, LE15 9NY",
    "orderItems": [
      {
        "product": "'I love my pet' t-shirt",
        "quantity": 1,
        "priceEach": 12.5
      },
      {
        "product": "'I love my pet' t-shirt",
        "quantity": 1,
        "priceEach": 15.99
      }
    ],
    "deliveryExpected": "2021-05-07T00:00:00"
  }
}`

## Unit Test
Used `MS Unite test` and `Moq` for writing unit test.

## Improvements/Extra miles
Below improvements can be implemented if I get more time,
 - I can build token based authentication.
 - If provided with more time I could write more Unit tests for more code coverage
 - Implement Logging in detail
 - Can build validation using Fluent validation
 - Can move hard coded API keys, endpoint to Appsettings.json.
 - Implement Automapper for mapping response

## Cloud Technologies to Consider
We can implement API using below cloud technologies,
 - API Gateway / App for hosting service in cloud
 - Docker/Containerization
