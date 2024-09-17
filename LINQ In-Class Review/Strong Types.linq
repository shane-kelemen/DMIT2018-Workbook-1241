<Query Kind="Program">
  <Connection>
    <ID>afa19593-0e52-43a2-9f86-5d7601f914d6</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>Contoso</Database>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

// LINQ Strongly Typed Queries

void Main()
{
	// Question 01
	// Testing method call
	GetEmployeeReview("al", 30).Dump();

	// Question 02
	// Testing method call
	GetProductColorProcess("Music").Dump();
}

// You can define other methods, fields, classes and namespaces here

// Question 02
// Create a method that retrieves product records based on a category name search. The method 
// should take a categoryName parameter and return a strongly typed list of 
// ProductColorProcessView objects, containing the product name, color, and whether additional
// color processing is needed, ordered by the product's style name.
private List<ProductColorProcessView> GetProductColorProcess (string categoryName)
{
	if (string.IsNullOrWhiteSpace(categoryName))
		throw new Exception("Provide a valid category name!");
	
	return Products
			.Where(product => product.ProductSubcategory.ProductCategory
											.ProductCategoryName.Contains(categoryName))
			.OrderBy(product => product.StyleName)
			.Select(product => new ProductColorProcessView
			{
				ProductName = product.ProductName,
				Color = product.ColorName,
				ColorProcessNeeded = product.ColorName == "Black" 
										|| product.ColorName == "White" ? "No" : "Yes"
			}).ToList();
}

// Question 02
// ViewModel
private class ProductColorProcessView
{
	public string ProductName { get; set; }
	public string Color { get; set; }
	public string ColorProcessNeeded { get; set; }
}



// Question 01
//	Create a method that retrieves employee records based on a search for last names and 
//	a base rate threshold. The method should take two parameters—lastName and baseRate—and 
//	return a strongly typed list of EmployeeView objects, containing the employee's full name, 
//	department, and income category, ordered by last name.
private List<EmployeeView> GetEmployeeReview (string lastName, decimal baseRate)
{
	return Employees
	.Where(employee => employee.LastName.Contains(lastName))
	.OrderBy(employee => employee.LastName)
	.Select(employee => new EmployeeView
	{
		FullName = employee.FirstName + " " + employee.LastName,
		Department = employee.DepartmentName,
		IncomeCategory = employee.BaseRate <= baseRate? "Review Required" : "No Review Required"
	})
	.ToList();
}

// Question 01
// ViewModel
private class EmployeeView
{
	public string FullName { get; set; }
	public string Department { get; set; }
	public string IncomeCategory  { get; set; }
}