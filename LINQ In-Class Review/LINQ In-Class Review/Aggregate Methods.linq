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

// Question 01
// How would you write a query to count the number of customers who have one or more children? 
// The query should filter the Customers table to find those with a TotalChildren value greater 
// than 0 and return the total count.
Customers
.Where(customer => customer.TotalChildren > 0)
.Count()
.Dump("Customers with Children");


// Question 02
// How would you write a query to count the number of employees whose base rate is greater than 
// $70 per hour? The query should filter the Employees table to find those with a BaseRate value
// greater than 70 and return the total count.
Employees
.Where(employee => employee.BaseRate > 70)
.Count()
.Dump("Employees earning mor than $70 per hour");


// Question 03
// How would you write a query to retrieve the name of each product and the total quantity on 
// hand? The query should calculate the sum of the OnHandQuantity for each product, and if a 
// product has no inventory records, it should return a quantity of 0, ordered by name. 
// NOTE: You will have to use the Ternary Operator to check if there is any inventory for the 
// product.
Products
.Select (product => new
{
	Name = product.ProductName,
	TotalOnHand = product.Inventories.Count() > 0?
					product.Inventories.Sum(inventory => inventory.OnHandQuantity) : 0
})
.OrderBy(anon => anon.Name)
.Dump("Product Total Quantity On Hand");


// Question 04
// How would you use LINQ to sum the total DiscountAmount for each promotion, ordered by 
// promotion name using navigation properties in the Promotion table?
Promotions
.Select(promotion => new
{
	PromotionID = promotion.PromotionID,
	PromotionName = promotion.PromotionName,
	TotalDiscountGiven = promotion.InvoiceLines.Sum(line => line.DiscountAmount),
	TotDisGiven = promotion.InvoiceLines.Select(line => line.DiscountAmount).Sum()
})
.OrderBy(anon => anon.PromotionName)
.Dump("Discount Sums by Promotion");


// Question 05
// How would you write a query to retrieve the lowest unit cost and price for each product 
// subcategory? The query should select the ProductCategory and ProductSubcategory names, 
// along with the minimum UnitCost and UnitPrice for products in each subcategory. The results
// should only include subcategories where both the lowest cost and price are available and 
// should be ordered by product category name.
ProductSubcategories
//***** This Where use works but it is ugly and repeats operations inside the select *****//
//.Where(subcat => subcat.Products.Min(product => product.UnitCost) != null
//					&& subcat.Products.Min(product => product.UnitPrice) != null)
.Select(subcat => new 
{
	Category = subcat.ProductCategory.ProductCategoryName,
	SubCategory = subcat.ProductSubcategoryName,
	LowestCost = subcat.Products.Min(product => product.UnitCost),
	LowestPrice = subcat.Products.Min(product => product.UnitPrice)
})
// ***** This Where is more desirable as it simplifies the code and eliminates doubling the operation ***** //
.Where(anon => anon.LowestCost != null && anon.LowestPrice != null)
.OrderBy(anon => anon.Category)
.Dump("Product SubCategory Lowest Costs and Proces");


// Question 06
// How would you write a query to retrieve the oldest invoice date for each store? The query
// should return the StoreID, StoreName, and the oldest invoice date formatted as a string. 
// If no invoices exist for a store, the result should display 'N/A' for the date. The results
// should be ordered alphabetically by the store name.
Stores
.Select(store => new 
{
	StoreID = store.StoreID,
	Name = store.StoreName,
	OldestInvoice = store.Invoices.Count() > 0 ?
				store.Invoices.Min(invoice => invoice.DateKey).ToString("M'/'d'/'yyyy") : "N/A"
})
.OrderBy(anon => anon.Name)
.Dump("Oldest Invoice Dates by Store");


// Question 07
// How would you write a query to retrieve the lowest and highest unit costs and prices for
// each product subcategory? The query should return the ProductCategory name, SubCategory 
// name, LowestCost, LowestPrice, MaxCost, and MaxPrice. The results should be filtered to 
// exclude subcategories without cost or price information and should be ordered by category
// name.
ProductSubcategories
.Select(subcat => new
{
	Category = subcat.ProductCategory.ProductCategoryName,
	SubCategory = subcat.ProductSubcategoryName,
	LowestCost = subcat.Products.Min(product => product.UnitCost),
	LowestPrice = subcat.Products.Min(product => product.UnitPrice),
	MaxCost = subcat.Products.Max(product => product.UnitCost),
	MaxPrice = subcat.Products.Max(product => product.UnitPrice),
})
.Where(anon => anon.LowestCost != null && anon.LowestPrice != null
				&& anon.MaxCost != null && anon.MaxPrice != null)
.OrderBy(anon => anon.Category)
.Dump("Product SubCategory Lowest and Highest Costs and Prices");


// Question 08
// How would you write a query to retrieve the largest invoice amount for each store? 
// The query should return the StoreID, StoreName, and the LargestInvoiceAmount. The results 
// should be ordered alphabetically by store name.
Stores
.Select(store => new
{
	StoreID = store.StoreID,
	Name = store.StoreName,
	LargestInvoiceAmount = store.Invoices.Count() > 0 ?
								store.Invoices.Max(invoice => invoice.TotalAmount) : 0
})
.OrderBy(anon => anon.Name)
.Dump("Largest Invoice Amount by Store");


// Question 09
// How would you write a query to calculate the average quantity of items sold for each 
// invoice? The query should return the InvoiceNo, InvoiceDate, and the AverageQty of items 
// sold per invoice.
Invoices
.Select(invoice => new
{
	InvoiceNo = invoice.InvoiceID,
	InvoiceDate = invoice.DateKey.ToString("M'/'d'/'yyyy"),
	AverageQty = (int)invoice.InvoiceLines.Average(line => line.SalesQuantity)
})
.Dump("Average Sales Quantity by Invoice");


// Question 10
// How would you write a query to calculate the average sales amount for each store? The 
// query should return the StoreID, Name, and the AverageSales for each store, ordered by 
// store name.
Stores
.Select(store => new 
{
	StoreID = store.StoreID,
	Name = store.StoreName,
	AverageSales = store.Invoices.Count() > 0 ?
					store.Invoices.Average(invoice => invoice.TotalAmount) : 0
})
.OrderBy(anon => anon.Name)
.Dump("Average Invoice Values by Store");
















