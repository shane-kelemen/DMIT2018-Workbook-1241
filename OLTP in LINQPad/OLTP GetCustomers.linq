<Query Kind="Program" />

#region Driver - Test Data for Testing Method Calls
void Main()
{
	
}
#endregion


#region Testing Method(s) - Destined to be the base for Client Side Code Behind

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

#endregion


#region View Models - Used for moving data between code behind pages and the System Library Methods

#endregion