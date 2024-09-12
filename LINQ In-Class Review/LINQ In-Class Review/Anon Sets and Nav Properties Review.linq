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

// Anonymous Sets with Navigation Properties In-Class Review

// Question 01
// How would you filter the Product table to retrieve products with a unit price less 
// than $10, order them by price and product name in ascending order, and return the 
// results as an anonymous data set that includes the product label, product name, and 
// unit price?
Product
.Where(product => product.UnitPrice < 10)
.OrderBy(product => product.UnitPrice)
.ThenBy(product => product.ProductName)
.Select(product => new
		{
			product.ProductLabel,
			product.ProductName,
			product.UnitPrice
		})
.Dump();

// Question 02
// How would you filter the Customer table to retrieve customers in British Columbia, 
// Canada, and include their first name, last name, and associated city name ordered 
// by city then by last name (from the Geography table) as an anonymous data set?
Customer
.Where(customer => customer.Geography.StateProvinceName == "British Columbia"
					&& customer.Geography.RegionCountryName == "Canada")
.Select(customer => new 
		{
			customer.FirstName,
			customer.LastName,
			customer.Geography.CityName
		})
.OrderBy(anon => anon.CityName)  // Remember that at this point, after the Select, 
								 //		you are no longer working with a 
								 // 	collection of the original type because of 
								 //		the anonymous type that was created.
.ThenBy(anon => anon.LastName)
.Dump();


// Question 03
// How would you filter the Products table to retrieve products that are 'Pink' 
// and belong to the 'Audio' category and any of the subcategories 'Recording Pen' 
// or 'Bluetooth Headphones', and return the results as an anonymous data set with 
// the product name, subcategory name, and category name?
Product
.Where(product => 
		product.ColorName == "Pink"
		&& (product.ProductSubcategory.ProductSubcategoryName == "Recording Pen"
		|| product.ProductSubcategory.ProductSubcategoryName == "Bluetooth Headphones")
		&& product.ProductSubcategory.ProductCategory.ProductCategoryName == "Audio")
.Select(product => new 
		{
			CategoryName = product.ProductSubcategory.ProductCategory.ProductCategoryName,
			SubcategoryName = product.ProductSubcategory.ProductSubcategoryName,
			ProductName = product.ProductName
		})
.OrderBy(anon => anon.SubcategoryName)
.ThenBy(anon => anon.ProductName)
.Dump();

// Same as above with multiple Where clauses
Product
.Where(product => product.ColorName == "Pink")
.Where(product => product.ProductSubcategory.ProductSubcategoryName == "Recording Pen"
					|| product.ProductSubcategory.ProductSubcategoryName == "Bluetooth Headphones")
.Where(product => product.ProductSubcategory.ProductCategory.ProductCategoryName == "Audio")		
.Select(product => new
{
	CategoryName = product.ProductSubcategory.ProductCategory.ProductCategoryName,
	SubcategoryName = product.ProductSubcategory.ProductSubcategoryName,
	ProductName = product.ProductName
})
.OrderBy(anon => anon.SubcategoryName)
.ThenBy(anon => anon.ProductName)
.Dump();


// Question 04
// How would you filter the Invoices table to retrieve invoices for customers located in 
// Europe and return the results as an anonymous data set that includes the invoice number,
// invoice date, customer name, city, and country, ordered by city?
Invoice
.Where(invoice => invoice.Customer.Geography.ContinentName == "Europe")
.Select(invoice => new 
		{
			InvoiceNo = invoice.InvoiceID,
			InvoiceDate = invoice.DateKey.ToString("yyyy-mm-dd"),
			CustomerName = invoice.Customer.FirstName + " " + 
							invoice.Customer.LastName,
			City = invoice.Customer.Geography.CityName,
			Country = invoice.Customer.Geography.RegionCountryName
		})
.OrderBy(anon => anon.City)
.Dump();


					

















