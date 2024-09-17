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

// Nested Queries
// THe biggest thing to remember with nested queries is that you must link the parent query to 
// the child query.  Failure to do so will cause all the result of the child query to be 
// unfiltered and repeated in their entirety for every item in the parent query.


void Main()
{
	// Question 01
	// Testing method call
	GetProductCategories().Dump();

	// Question 02
	// Testing method call
	GetInvoicesWithDetails("Torres").Dump();
}

// You can define other methods, fields, classes and namespaces here


// Question 02
// Create a method that retrieves a detailed view of all invoices for a customer based on a 
// specified last name. The method should return a strongly typed list of InvoiceView objects,
// where each InvoiceView contains the invoice number, invoice date, customer name, total 
// amount, and a strongly typed list of InvoiceLineView objects. Each InvoiceLineView should 
// represent a line item on the invoice, including the product name, quantity, price, 
// discount, and extended price. The invoice lines should be ordered by their line reference.
private List<InvoiceView> GetInvoicesWithDetails (string lastName)
{
	return Invoices
			.Where(invoice => invoice.Customer.LastName.Contains(lastName))
			.Select(invoice => new InvoiceView
			{
				InvoiceNo = invoice.InvoiceID,
				InvoiceDate = invoice.DateKey.ToShortDateString(),
				Customer = $"{invoice.Customer.FirstName} {invoice.Customer.LastName}",
				Amount = invoice.TotalAmount,
				Details = invoice.InvoiceLines
							.Select(line => new InvoiceLineView
							{
								LineReference = line.InvoiceLineID,
								ProductName = line.Product.ProductName,
								Qty = line.SalesQuantity,
								Price = (decimal)line.UnitPrice,
								Discount = (decimal)line.DiscountAmount,
								ExtPrice = (decimal)line.SalesAmount
							})
							.OrderBy(ilv => ilv.LineReference)
							.ToList()
			})
			.ToList();
}


// Question 02
// View Models
private class InvoiceView
{
	public int InvoiceNo { get; set; }
	public string InvoiceDate { get; set; }
	public string Customer { get; set; }
	public decimal Amount { get; set; }
	public List<InvoiceLineView> Details { get; set; }
}


private class InvoiceLineView
{
	public int LineReference { get; set; }
	public string ProductName  { get; set; }
	public int Qty { get; set; }
	public decimal Price { get; set; }
	public decimal Discount { get; set; }
	public decimal ExtPrice { get; set; }
}





// Question 01
// How would you create a strongly typed query that retrieves all product categories and their 
// associated subcategories as strongly typed lists? The query should return a list of 
// ProductCategorySummaryView & ProductSubcategorySummaryView objects, where each category 
// includes its name and a list of subcategories. Each subcategory should include its name 
// and description. The results should be ordered first by product category name and then by 
// subcategory name.
private List<ProductCategorySummaryView> GetProductCategories()
{
	return ProductCategories
			.Select(category => new ProductCategorySummaryView
			{
				ProductCategoryName = category.ProductCategoryName,
				SubCategories = category.ProductSubcategories
								.Select(sub => new ProductSubcategorySummaryView
								{
									SubcategoryName = sub.ProductSubcategoryName,
									Description = sub.ProductSubcategoryDescription
								})
								.OrderBy(pssv => pssv.SubcategoryName)
								.ToList()
			})
			.OrderBy(pcsv => pcsv.ProductCategoryName)
			.ToList();
	
}

// Question 01
// View Models
private class ProductCategorySummaryView
{
	public string ProductCategoryName { get; set; }
	public List<ProductSubcategorySummaryView> SubCategories { get; set; }
}

private class ProductSubcategorySummaryView
{
	public string SubcategoryName { get; set; }
	public string Description { get; set; }
}