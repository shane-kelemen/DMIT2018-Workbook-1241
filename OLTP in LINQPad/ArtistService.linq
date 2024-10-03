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
	#region Test GetArtist
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
	#endregion

	
}
#endregion


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
#endregion


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