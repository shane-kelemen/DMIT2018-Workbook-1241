<Query Kind="Program">
  <Connection>
    <ID>e1311e81-54f4-47eb-9ab9-e3732af60377</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>ChinookSept2018</Database>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

#region Driver - Test Data for Testing Method Calls
void Main()
{
	// Include a region for testing each pair of Testing Method / System Library Method
	// In the Blazor app, these values would be extracted from the client side UI controls.
	
	#region Test GetArtist
	Console.WriteLine("***********************************************");
	Console.WriteLine("Begin GetArtist Testing");
	
	Console.WriteLine("Test Fail - Invalid Artist ID = 0");
	// Test an illegal artist ID
	TestGetArtist(0);
	Console.WriteLine();
	
	Console.WriteLine("Test Fail - Valid Artist ID = 1000 - Null result");
	// Test a legal artist ID with a null result
	TestGetArtist(1000);
	Console.WriteLine();

	// Test retrieving an artist with a valid ArtistID
	TestGetArtist(1).Dump("Test Pass - Valid Artist ID = 1");
	Console.WriteLine();
	
	Console.WriteLine("End GetArtist Testing");
	Console.WriteLine("***********************************************");
	#endregion
	
	Console.WriteLine();
	
	#region Test GetArtistsByName
	Console.WriteLine("***********************************************");
	Console.WriteLine("Begin GetArtistsByName Testing");
	
	Console.WriteLine("Fail Tests - Input invalid strings");
	TestGetArtistsByName(null);
	TestGetArtistsByName("");
	TestGetArtistsByName("   ");
	TestGetArtistsByName("\t");
	Console.WriteLine();
	
	Console.WriteLine("Pass Tests - Valid partial names");
	TestGetArtistsByName("ABB").Dump("Pass - Found Artist Name");
	TestGetArtistsByName("ABC").Dump("Pass - Artist Name Not Found");
	Console.WriteLine();
	
	Console.WriteLine("End GetArtistsByName Testing");
	Console.WriteLine("***********************************************");
	#endregion
}
#endregion


// Note how each testing method takes the input data and passes it through to the 
// system library testing method.  The pattern for catching any thrown exceptions is the same 
// for each testing method.
#region Testing Method(s) - Destined to be the base for Client Side Code Behind
public ArtistEditView TestGetArtist (int artistID)
{
	ArtistEditView aev = new ArtistEditView();
	
	try
	{
		aev = GetArtist(artistID);
	}
	catch (AggregateException ae)
	{
		ae.Message.Dump();
		foreach(var error in ae.InnerExceptions)
		{
			error.Message.Dump();   // This will change to become code for displaying
									// exception messages to the user.
		}
	}
	catch (ArgumentNullException ane)
	{
		GetInnerException(ane).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();	
	}
	
	return aev;
}

public List<ArtistEditView> TestGetArtistsByName (string partialName)
{
	List<ArtistEditView> artists = new List<ArtistEditView>();

	try
	{
		artists = GetArtistsByName(partialName);
	}
	catch (AggregateException ae)
	{
		ae.Message.Dump();
		foreach (var error in ae.InnerExceptions)
		{
			error.Message.Dump();   // This will change to become code for displaying
									// exception messages to the user.
		}
	}
	catch (ArgumentNullException ane)
	{
		GetInnerException(ane).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	return artists;
}
#endregion


// Again, the pattern for the methods should be consistent.  First data validation,
// then business rules, then finally actual method operations.  Start each data validation
// section with the creation of a List<Exception> to which all messages to the user will
// be added.  End each data validation section with a check for list count.  If greater than 
// zero, throw an aggregate exception, passing out the list of exceptions.
#region System Library Methods - LINQ Interactions with Entity Framework
public ArtistEditView GetArtist(int artistID)
{
	#region Data Validation and Business Logic Application
	// create a list for holding our exceptions when validation data
	List<Exception> errorList = new List<Exception>();
	
	if (artistID <= 0)
	{
		errorList.Add(new ArgumentException("Artist ID must be 1 or greater!"));
	}
	
	if (errorList.Count > 0)
	{
		throw new AggregateException("Could not retrieve artist.  Check errors:", errorList);
	}
	#endregion
	
	ArtistEditView aev =  Artists
							.Where(artist => artist.ArtistId == artistID)
							.Select(artist => new ArtistEditView
							{
								ArtistID = artist.ArtistId,
								Name = artist.Name
							})
							.FirstOrDefault(); 
	
	if (aev == null)
	{
		throw new ArgumentNullException("There was no matching artist for the provided artist ID!");
	}
	
	return aev;
}

public List<ArtistEditView> GetArtistsByName (string partialName)
{
	List<ArtistEditView> artists = new List<ArtistEditView>();

	#region Data Validation and Business Logic Application
	// create a list for holding our exceptions when validation data
	List<Exception> errorList = new List<Exception>();

	// All data validation should add an exception to the collection if the data is determined to be invalid
	if (string.IsNullOrWhiteSpace(partialName))
	{
		errorList.Add(new ArgumentException("You must provide a partial name that is not null, empty, or whitespace!"));
	}

 	// After the data validations are complete, if the collection holds any exceptions, throw
	// the aggregate exception and pass out the errorList.
	if (errorList.Count > 0)
	{
		throw new AggregateException("Could not retrieve artist.  Check errors:", errorList);
	}
	#endregion
	
	// If no exceptions were thrown, perform the needed operations.
	artists = Artists
				.Where(artist => artist.Name.ToLower().Contains(partialName.ToLower()))
				.Select(artist => new ArtistEditView
				{
					ArtistID = artist.ArtistId,
					Name = artist.Name
				})
				.ToList();

	return artists;
}
#endregion


#region Support (Helper) Methods - To support operation in Testing / System Library Methods
public Exception GetInnerException(Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;

	return ex;
}
#endregion


#region View Models - Used for moving data between code behind pages and the System Library Methods
public class ArtistEditView
{
	public int ArtistID { get; set; }
	public string Name { get; set; }
}
#endregion


























