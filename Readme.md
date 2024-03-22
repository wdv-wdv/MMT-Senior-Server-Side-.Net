# MMT Senior Server Side .Net Technical Test
## The assignment 

The assignment is to create an API which combines two sources of data from a API and a database for consumption by a front-end application.

It was coded using the Visual Studio 2019 IDE. To open, look for the solution file (**MMT_Test_WDV.sln**) in the repo root

Inside the solution there are three projects.

* **EcommerceAPI** start-up project
* **EcommerceCommon** for holding data layer and business logic classes
* **EcommerceTest** Unit tests

I would suggest running the Unit test first.

## Data access components (EcommerceCommon)

### OrderDB

I opted to go with Entity framework. Used the `Scaffold-DbContext` command to build the data models from the existing database. Access is provided via a singleton added to the services object for use by controllers dependency injection mechanism

### Customer details API

This is a simply HttpClient HTTP requests process. Access is provided via a singleton added to the services object for use by controllers dependency injection mechanism

## API Architecture

The API accept two variables: user (email) and custormerId

Endpoint: **POST** `/v1/orders/LastOrders`

Internal logic flow:

1.	Retrieves customer details from customer details API
2.	Compares submitted *CustomerID* with *CustomerID* retrieved customer details API. Returns 404 when not matching 
3.  Retrieves *Order* details from OrderDB
4.	Creates a Anonymous Type *Customer Order* object which will act as payload for the API
5.	Add *Order* object property with Address from customer details and handel the is a gift logic
6.	Add *Customer* object property (Fisrtname, lastname)
7.	Return *Customer Order* object

## Testing 

I create four tests. Two for testing the data-layer integration between the app and Order database, as well as between the app and the customer details API

* Datalayer_GetCustomer
* Datalayer_GetOrder

And two for testing to the basic outcomes on the API controller self.

* API_LastOrders
* API_LastOrders_Invalid_CustomerID

Unit tests don’t have the concept of an appSettings.json file. For this I use the Test_Helper class I created to load and bind the setting for the API-key and database connection string

To test the API self, I used these curl commands below in a GitBash window

```
curl -d "{ \"user\": \"cat.owner@mmtdigital.co.uk\",\"customerId\": \"C34454\"}" -w " %{http_code}" -H "Content-Type: application/json" "http://localhost:65110/v1/orders/lastorders"
```

```
curl -d "{ \"user\": \"bob@mmtdigital.co.uk\",\"customerId\": \"R223232\"}" -w " %{http_code}" -H "Content-Type: application/json" "http://localhost:65110/v1/orders/lastorders"
```

## Thought on deployment

These days everything gets deployed via a CICD process.  Apart for it being a good way of testing your code in a real-world environment, it is also useful for protecting sensitive configuration data like API keys and database connection string using a secret manager feature.  

I stored the once provided in an AppSettings.json file. When moving the app into a production environment the CICD can overwrite the AppSettings.json file with production credentials, or load it into a system environment variable which overwrites AppSettings.json file values as  well.

It is also advisable not commit the AppSettings.json to source control at all by adding it to the .gitignore 

## Thought on changes

* I would like to change the setup so that the deliver address is added to the order database at the time the order is placed, instead of using the customer details API value.
* The spec don’t specify so assumed that the Product field should include Colour and size with the product name, I would have wanted to follow that up with the front-end team.
* Maybe add a few more tests to check data integrity
    * Number of order items would like be at least one item
    * Required fields have values. 

  
