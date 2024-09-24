<Query Kind="Statements">
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

// In-Class Practice GroupBy()

// Question 01
// "How would you write a query to group product subcategories by their parent category? 
// The query should return the CategoryName and a list of ProductSubcategories that includes the 
// SubCategoryName. Each subcategory should be ordered alphabetically by category name and subcategory name."
ProductSubcategories
.GroupBy(subcat => new { subcat.ProductCategory.ProductCategoryName })
//.GroupBy(subcat => subcat.ProductCategory.ProductCategoryName )
// Remember that at this point you are dealing with subgroups of the original collection (table in this case)
// Because the key was set up as an anonymous set, the OrderBy must be completed before the Select.  If the 
// Key is set up with just a single value as in the commented up GroupBy() above, then the OrderBy() may
// come after the Select().  This has to do with how the query manager builds the underlying query.
// The OrderBy() may also come after the Select if the Key column name is specified when using the anonymous
// set syntax in the GroupBy().
//.OrderBy(group => group.Key.ProductCategoryName)
.Select(group => new 
{
	// CategoryName = group.Key,
	CategoryName = group.Key.ProductCategoryName,
	ProductSubcategories = group.Select(subcat => new 
								 {
								 	ProductSubcategories = subcat.ProductSubcategoryName
								 })
								 .OrderBy(subAnon => subAnon.ProductSubcategories)
								 .ToList()
})
.OrderBy(anon => anon.CategoryName)
.Dump();


// Question 02
// "How would you write a query to group invoice lines by product category and subcategory? 
// The query should return the CategoryName, SubcategoryName, and a list of invoices that include the 
// InvoiceID, Product, and Amount. For each product, order by category name, subcategory name, and 
// finally by product name."
InvoiceLines
.GroupBy(line => new 
{
	line.Product.ProductSubcategory.ProductCategory.ProductCategoryName,
	line.Product.ProductSubcategory.ProductSubcategoryName
})  // Remember that at this point you are dealing with subgroups of the original collection (table in this case)
.Select(group => new 
{
	CategoryName = group.Key.ProductCategoryName,
	SubcategoryName = group.Key.ProductSubcategoryName,
	Invoices = group.Select(line => new 
					 {
					 	InvoiceID = line.InvoiceID,
						Product = line.Product.ProductName,
						Amount = line.SalesQuantity
					 })
					 .OrderBy(subAnon => subAnon.Product)
					 .ToList()  // Required when using the EF connection
})
.OrderBy(anon => anon.CategoryName)
.ThenBy(anon => anon.SubcategoryName)
.Dump();

































