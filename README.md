# dotNettbank

2 parts grup and 1 part individual assignment for the subject 'Webapplikasjoner' at HiOA.

## Key functionality
* Secure login with user sessions
* Client and server validation of input fields
* MVC architeture with seperated DAL and BLL layers
* Bootstrap styling
* Dynamic pages using Jquery and Ajax

## Manual

### Initial database
The solution creates a database using Entity Framework Code First. This will be created and initialized if no current database exists. Using a DatabaseInitializer, the solution will create a database and populate it with data in all tables.

Two test users/customers are created. These can be used to log in to the website using these credentials:
#### Customer 1
* Personnummer: 01018912345
* Passord: Test123

#### Customer 2
* Personnummer: 01010199887
* Passord: Test123

### Login
* Secure 3 step login starts at page ~/Home/LoginBirth. 
* Birthnumber enterd must be of valid format
* The BankId step uses only a dummy and accepts any combination of 6 numbers.
* The password needs to be at 7 characters.
A session keeps track of the logged in status.

### Register Customer

A new customer can register at page ~/Customer/RegisterCustomer.


## Current Bugs

At AccountStatement (~/Customer/AccountStatement) date range comparison against transaction date does not work 100% properly with the initial values in the two datepickers (Fra dato & Til dato). It does however work as intended once the user selects a value.

To see a larger range of transactions from the initialized database, please select two dates relatively far apart (and in to the future)
