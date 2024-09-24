<Query Kind="Statements">
  <Connection>
    <ID>3aedc78a-93a3-4e24-b20b-4a3d76bcc54f</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Contoso</Database>
    <NoPluralization>true</NoPluralization>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

// Queries from theWhere() In-Class Review

// Question 01
// "We need to identify all employees hired after January 1, 2022, 
// 		to ensure they are included in our new training program."
Employee
.Where(employee => employee.HireDate > new DateOnly(2022, 01, 01))
.Dump();

//Question 02
//"How would you filter the Product table to retrieve these products?"
//"Our inventory team wants to find all products that have been available 
//		for sale since July 1, 2019, to ensure they are properly stocked."
Product
.Where(product => product.AvailableForSaleDate > new DateTime(2019, 07, 01))
.Dump();

// Question 03
// "To update our customer database, we need to pull the email addresses 
//		of all customers with a yearly income between $60,000 & $61,000."
// NOTE: The author of the question used a non-inclusive upper range value,
//			yielding only 119 results.  This is contrary to how the keyword 
//			between would work in SQL which is inclusive both lower and upper.
Customer
.Where(customer => customer.YearlyIncome >= 60000
		&& customer.YearlyIncome <= 61000)
.Select(customer => customer.EmailAddress)
.Dump();

// Question 04
// "The marketing department needs a list of all promotions focused on 
//		North America to prepare for the upcoming sale."
Promotion
.Where(promotion => promotion.PromotionName.Contains("North America"))
.Dump();