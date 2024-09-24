<Query Kind="Program">
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

// Queries from the OrderBy() In-Class Review

// NOTE: At first I thought that this using statement was needed because the List<> was not recognizing the "Employees" as a data type,
//	even though Employees was listed as a type under our connections on the left.  It was discovered that using the singular versions,
// 	like Employee, fixed the unrecognized data type issue, and so further testing shows that this using statement was not the underlying
// 	issue after all.  It may be safely commented out and the rest of the code will still run.

// using System.Collections.Generic;

void Main()
{
	// Question 01
	// "We need to identify all employees hired after January 1, 2022,
	//		to ensure they are included in our new training program.
	//		Ordered by Last Name."
	
	// These first two lines of code show an explicit way to catch the result sets from the database.  This will be desired
	//		in some contexts later in the course.  For now just chaining the Dump() is fine.
	//List<Employee> myEmployees = GetEmployees();
	//myEmployees.Dump();
	
	GetEmployees().Dump();
	
	// Question 02
	// "Our inventory team wants to find all products that have been available 
	// 		"for sale since July 1, 2019, to ensure they are properly stocked.
	//		Ordered by descending product label."
	GetProducts().Dump();
	
	// Question 03
	// "To update our customer database, we need to pull the email addresses 
	//		of all customers with a yearly income between $60,000 and $61,000." 
	// 		Order by email address.
	GetCustomers().Dump();
	
	// Question 04
	// "The marketing department needs a list of all promotions focused on North 
	//		America to prepare for the upcoming sale."
	// 		Order by promotion name.
	GetPromotions().Dump();
}

// Note that the LINQ queries in the methods below have been changed to using
// 	the singular form of the data type derived from the database tables.  So Employee instead of Employees.  This change was necessary 
//	because I changed the connection involved to NOT pluralize the database table names.  IF YOU DO NOT MAKE THIS CHANGE to your 
//	connection on the left (where singular names are populated instead of pluralized names) then your LINQ queries will need to use
//	the pluralized names, but your List<> collections will need to use singular names.  Either is fine with me... choose what you are 
//  comfortable with.

// Question 01
List<Employee> GetEmployees()
{
	return Employee
		   .Where(employee => employee.HireDate > new DateOnly(2022, 01, 01))
		   .OrderBy(employee => employee.LastName)
		   .ToList();
}

// Question 02
List<Product> GetProducts()
{
	return Product
		   .Where(product => product.AvailableForSaleDate > new DateTime(2019, 07, 01))
		   .OrderByDescending(product => product.ProductLabel)
		   .ToList();

}

// Question 03
List<string> GetCustomers()
{
	return Customer
		   .Where(customer => customer.YearlyIncome >= 60000
					&& customer.YearlyIncome <= 61000)
		   .OrderBy(customer => customer.LastName)
		   .Select(customer => customer.EmailAddress)
		   .ToList();

}

// Question 04
List<Promotion> GetPromotions()
{
	return Promotion
		   .Where(promotion => promotion.PromotionName.Contains("North America"))
		   .OrderBy(promotion => promotion.PromotionName)
		   .ThenBy(promotion => promotion.StartDate)
		   .ToList();
	
}



