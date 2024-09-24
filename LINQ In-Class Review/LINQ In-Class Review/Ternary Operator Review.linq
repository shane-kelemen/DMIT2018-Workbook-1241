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

// Ternary Operator In-Class Exercises

// Question 01
// How would you filter the Employees table to retrieve those with a base rate of less than $30,
// and return the results as an anonymous data set that includes their full name, department, 
// and whether they require a salary review, ordered by last name?
Employee
.OrderBy(employee => employee.LastName)
.Select(employee => new
		{
			FullName = $"{employee.FirstName} {employee.LastName}",
			Department = employee.DepartmentName,
			IncomeCategory = employee.BaseRate < 30 ? "Review Required" : "No Review Required"
		})
.Dump();


// Question 02
// How would you filter the Products table to retrieve items in the 
// 'Music, Movies, and Audio Books' category, and return the results as an anonymous data 
// set that includes the product name, color, and whether color processing is needed, 
// ordered by style name?
Product
.Where(product =>
	product.ProductSubcategory.ProductCategory.ProductCategoryName == "Music, Movies and Audio Books")
.OrderBy(product => product.StyleName)
.Select(product => new 
		{
			ProductName = product.ProductName,
			Color = product.ColorName,
			ColorProcessNeeded = product.ColorName == "White" || product.ColorName == "Black" ? "No" : "Yes"
		})
.Dump();

