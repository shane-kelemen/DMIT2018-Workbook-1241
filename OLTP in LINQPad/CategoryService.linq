<Query Kind="Program">
  <Connection>
    <ID>b8a83fe6-e4a6-4386-adbd-a9922623bd77</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>OLTP-DMIT2018</Database>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

//	Driver is responsible for orchestrating the flow by calling 
//	various methods and classes that contain the actual business logic 
//	or data processing operations.
void Main()
{
	#region Get Category
	//  Header information
	Console.WriteLine("==================");
	Console.WriteLine("=====  Get Category Pass  =====");
	Console.WriteLine("==================");
	//  Pass
	TestGetCategory("Province").Dump("Pass - Valid Category name");
	TestGetCategory("People").Dump("Pass - Valid name - No Category found");

	//  Header information
	Console.WriteLine();
	Console.WriteLine("==================");
	Console.WriteLine("=====  Get Category Fail  =====");
	Console.WriteLine("==================");
	//  Fail
	//  rule:	Category name cannot be empty or null
	TestGetCategory(string.Empty).Dump("Fail - Category name was empty");
	TestGetCategory(null).Dump("Fail - Category name was null");
	TestGetCategory("    ").Dump("Fail - Category name was whitespace");
	#endregion

	#region Add New Category
	//  Header information
	Console.WriteLine("==================");
	Console.WriteLine("=====  Add New Category  =====");
	Console.WriteLine("==================");

	//  create a new category view model for adding/editing
	CategoryView categoryView = new CategoryView();

	//  create a placeholder for the category name
	string categoryName = string.Empty;

	//	update the category name with the placeholder
	categoryView.CategoryName = categoryName;

	//  get last 5 records from the category table 
	//	before adding new category record
	Categories.Skip(Categories.Count() - 5).Dump("Return last 5 records before adding new category");

	//  Fail
	Console.WriteLine("==================");
	Console.WriteLine("=====  Add New Category Fail =====");
	Console.WriteLine("==================");

	//  rule: 	category name is required
	TestAddEditCategory(categoryView).Dump("Fail - Category name was empty");

	//  Pass
	Console.WriteLine("==================");
	Console.WriteLine("=====  Add New Category Pass =====");
	Console.WriteLine("==================");

	//	update the category name with a valid random name
	categoryName = GenerateName(6);
	categoryView.CategoryName = categoryName;
	TestAddEditCategory(categoryView).Dump($"Pass - Category has valid name: {categoryName}");

	//  get last 5 records from the category table 
	//	after adding new category record
	Categories.Skip(Categories.Count() - 5).Dump("Return last 5 records after adding new category");

	//  Fail
	Console.WriteLine("==================");
	Console.WriteLine("=====  Add New Category Fail =====");
	Console.WriteLine("==================");

	//  create a new category view model for adding/editing
	//	required so that we have a categoryID of 0
	categoryView = new CategoryView();

	//	update the category name with an category name
	categoryView.CategoryName = categoryName;

	//  Fail
	//  rule:	category cannot be duplicated (found more than once)
	TestAddEditCategory(categoryView).Dump($"Fail - Category {categoryName} already exist");
	#endregion

	#region Edit Category
	//  Header information
	Console.WriteLine("==================");
	Console.WriteLine("=====  Edit Existing Category  =====");
	Console.WriteLine("==================");

	//  get last 5 records from the category table 
	//	before editing existing category record
	Categories.Skip(Categories.Count() - 5).Dump("Return last 5 records before editing existing category");

	//  get existing category using categoryname placeholder
	string previousCategoryName = categoryName;
	categoryView = GetCategory(categoryName);

	//  validate that the category view is the one that we want.
	categoryView.Dump($"Valid category has category name of {categoryName}");

	//  Pass
	Console.WriteLine("==================");
	Console.WriteLine("=====  Edit Category Pass =====");
	Console.WriteLine("==================");

	//  update category name with a 10 character name
	categoryName = GenerateName(10);
	categoryView.CategoryName = categoryName;
	TestAddEditCategory(categoryView).Dump($"Pass - Category name has been updated from {previousCategoryName} to {categoryName}");

	//  get last 5 records from the category table 
	//	after editing existing category record
	Categories.Skip(Categories.Count() - 5).Dump("Return last 5 records after editing existing category");
	#endregion

	#region Soft Delete
	//  Header information
	Console.WriteLine("==================");
	Console.WriteLine("=====  Soft Delete =====");
	Console.WriteLine("==================");

	//  get existing category using categoryname placeholder
	categoryView = GetCategory(categoryName);
	//  set remove from view flag to true
	categoryView.RemoveFromViewFlag = true;

	TestAddEditCategory(categoryView).Dump("Pass - Remove from view flag has been updated");

	//  get last 5 records from the category table 
	//	after editing existing category record
	Categories.Skip(Categories.Count() - 5).Dump("Return last 5 records after updating remove from view flag");
	#endregion
}

//	This region contains methods used for testing the functionality
//	of the application's business logic and ensuring correctness.
#region Test Methods
//  test GetCategory
public CategoryView TestGetCategory(string categoryName)
{
	//CategoryView cv = new CategoryView();
	
	try
	{
		return GetCategory(categoryName);
	}
	#region  catch all exceptions (define later)
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	#endregion
	
	return null;  // Ensures a valid return value even on failure
}


//  test AddEditCategory
public CategoryView TestAddEditCategory(CategoryView categoryView)
{
	try
	{
		return AddEditCategory(categoryView);
	}
	#region  catch all exceptions (define later)
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	#endregion
	return null;  // Ensures a valid return value even on failure
}
#endregion


//	This region contains support methods for testing
#region Support Methods
public Exception GetInnerException(System.Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}

/// <summary>
/// Generates a random name of a given length.
/// The generated name follows a pattern of alternating consonants and vowels.
/// </summary>
/// <param name="len">The desired length of the generated name.</param>
/// <returns>A random name of the specified length.</returns>
public string GenerateName(int len)
{
	// Create a new Random instance.
	Random r = new Random();

	// Define consonants and vowels to use in the name generation.
	string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
	string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };

	string Name = "";

	// Start the name with an uppercase consonant and a vowel.
	Name += consonants[r.Next(consonants.Length)].ToUpper();
	Name += vowels[r.Next(vowels.Length)];

	// Counter for tracking the number of characters added.
	int b = 2;

	// Add alternating consonants and vowels until we reach the desired length.
	while (b < len)
	{
		Name += consonants[r.Next(consonants.Length)];
		b++;
		Name += vowels[r.Next(vowels.Length)];
		b++;
	}

	return Name;
}
#endregion

//	This region contains all methods responsible 
//	for executing business logic and operations.
#region Methods
//  Get category
public CategoryView GetCategory(string categoryName)
{
	#region Business Logic and Parameter Exceptions
	//	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//  Business Rules
	//	These are processing rules that need to be satisfied
	//		for valid data
	//		rule: 	category name is required
	if (string.IsNullOrWhiteSpace(categoryName))
	{
		throw new ArgumentNullException("Category name is required");
	}
	#endregion

	
	return Categories
			.Where(x => x.CategoryName == categoryName
						&& x.RemoveFromViewFlag == false)
			.Select(x => new CategoryView
			{
				CategoryID = x.CategoryID,
				CategoryName = x.CategoryName,
				RemoveFromViewFlag = x.RemoveFromViewFlag
			}).FirstOrDefault();
			
}

//  Add or edit a new or existing category
public CategoryView AddEditCategory(CategoryView categoryView)
{
	#region Business Logic and Parameter Exceptions
	//	create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//  Business Rules
	//	These are processing rules that need to be satisfied
	//		for valid data
	//		rule:	categoryView cannot be null	
	//		rule: 	category name is required
	if (categoryView == null)
	{
		throw new ArgumentNullException("No category was supply");
	}


	if (string.IsNullOrWhiteSpace(categoryView.CategoryName))
	{
		errorList.Add(new Exception("Category name is required"));
	}

	//		rule:	category cannot be duplicated (found more than once)
	if (categoryView.CategoryID == 0)
	{
		bool categoryExist = Categories
						.Where(x => x.CategoryName == categoryView.CategoryName)
						.Any();

		if (categoryExist)
		{
			errorList.Add(new Exception("Category already exist in the database and cannot be enter again"));
		}
	}
	#endregion

	// check to see if the category exist in the database
	Category category =
			Categories.Where(x => x.CategoryID == categoryView.CategoryID)
			.Select(x => x).FirstOrDefault();

	//  if the category was not found (CategoryID == 0)
	//		then we are dealing with a new category
	if (category == null)
	{
		category = new Category();
	}

	// 	Updating category name.
	//	NOTE:  	You do not have to update the primary key "CategoryID".
	//				This is true for all primary keys for any view models.
	//			- If it is a new category, the CategoryID will be "0"
	//			- If it is an existing category, there is no need
	//				to update it.

	category.CategoryName = categoryView.CategoryName;

	//	You must set the RemoveFromViewFlag incase it has been soft delete
	category.RemoveFromViewFlag = categoryView.RemoveFromViewFlag;

	//	If there are errors present in the error list:
	//	NOTE:  YOU CAN ONLY HAVE ONE CHECK FOR ERRORS AND SAVE CHANGES
	//			  IN A METHOD
	if (errorList.Count > 0)
	{
		// 	Clearing the "track changes" ensures consistency in our entity system.
		//	Otherwise we leave our entity system in flux
		ChangeTracker.Clear();
		//	Throw an AggregateException containing the list of business processing errors.
		throw new AggregateException("Unable to add or edit category. Please check error message(s)", errorList);
	}
	else
	{
		if (category.CategoryID == 0)
			//	Adding a new category record to the Category table
			Categories.Add(category);
		else
			//	Updating an category record in the Category table
			Categories.Update(category);

		//	NOTE:  YOU CAN ONLY HAVE ONE SAVE CHANGES IN A METHOD	
		SaveChanges();
	}
	return GetCategory(categoryView.CategoryName);
}
#endregion

//	This region includes the view models used to 
//	represent and structure data for the UI.
#region View Models
public class CategoryView
{
	public int CategoryID { get; set; }
	public string CategoryName { get; set; }
	public bool RemoveFromViewFlag { get; set; }
}

#endregion

