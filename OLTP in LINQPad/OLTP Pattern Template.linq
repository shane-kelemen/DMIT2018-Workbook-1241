<Query Kind="Program" />

#region Driver - Test Data for Testing Method Calls
void Main()
{
	// Setting up batches of test data to use for calling out testing methods
}
#endregion


#region Testing Method(s) - Destined to be the base for Client Side Code Behind
public ViewModel Test_SomeMethod(/* Input data required goes here  */)
{
	ViewModel vm = new ViewModel();  // View model carrying the data to be returned

	try
	{
		// Specific calling code for the method being tested
		// This will populate the view model if successful 
		vm = GetData(/* Input data required goes here  */);
	}
	catch (AggregateException ae)
	{
		ae.Message.Dump();  // General message in the aggregate exception
		foreach (var error in ae.InnerExceptions)
		{
			error.Message.Dump();   // This will change to become code for displaying
									// 	 exception messages to the user.
									// Displays each exception from the collection
									//	 of InnerExceptions in the AggregateException
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
	
	return vm;	// return the view model carrying the information
}
#endregion


#region Support (Helper) Methods - To support operation in Testing / System Library Methods
public Exception GetInnerException(Exception ex)
{
	while(ex.InnerException != null)
		ex = ex.InnerException;
	
	return ex;
}
#endregion


#region System Library Methods - LINQ Interactions with Entity Framework
public ViewModel GetData(/* Input data required goes here  */)
{
	ViewModel vm = new ViewModel();
	
	#region Data Validation and Business Logic Application
	List<Exception> errorList = new List<Exception>();

	// Tests for validation the data, and common business rules
	// When a test fails, add an Exception to the errorlist
	// if( test condition indicates failure)
	//		errorList.Add(new Exception ("Some properly descriptive error message!"));

	if (errorList.Count > 0)
	{
		throw new AggregateException("General descriptive message.  Check errors:", errorList);
	}
	#endregion
	
	// The processing code for the method goes after this note
	
	
	return vm;
}
#endregion


#region View Models - Used for moving data between code behind pages and the System Library Methods
public class ViewModel
{
	// Data properties for carrying data from place to place in the system
}
#endregion