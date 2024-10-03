<Query Kind="Program" />

#region Driver - Test Data for Testing Method Calls
void Main()
{
	// Set up batches of test data to use for calling out testing methods
	// Make sure to include the "happy path", or the successful test cases, but
	// also make sure you include every test case that will result in exceptions
	// being trown or other failures.
}
#endregion


// We create the testing methods as separate from main so that several calls with different
// sets of data may be initiated from main, covering all of the pass and fail cases.
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


// The database interactions will be performed through these library methods.  Before the 
// operations are performed, the data must be checked to ensure that it is valid, and business 
// rules must be enforeced.  Exceptions are thrown for any violation of data or business rules.
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
	
	// The processing code for the method goes after this note.
	// Example, a LINQ query might be completed.
	
	
	return vm;
}
#endregion


// Any support methods are also going to be consistent. Once defined they should not change and should be
// usable in multiple places within your projects.  These may include methods for supporting the code behind
// and / or the system libraries.  It is important to note that if needed in both projects, they should be
// defined once in each project, or placed in a third utility library.
#region Support (Helper) Methods - To support operation in Testing / System Library Methods
public Exception GetInnerException(Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;

	return ex;
}
#endregion


// Put all your View Models in this bottom section.  Once they are defined they will remain consistent 
#region View Models - Used for moving data between code behind pages and the System Library Methods
public class ViewModel
{
	// Data properties for carrying data from place to place in the system
}
#endregion