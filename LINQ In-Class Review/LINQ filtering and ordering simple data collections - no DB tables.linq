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

// Demoing LINQ queries against simple data sets

List<int> myInts = new List<int>();
for (int i = 0; i < 10; ++i)
	myInts.Add(i);
foreach(int num in myInts)
	Console.Write(num + " ");
Console.WriteLine();
	
foreach (int num in myInts.Where(x => x % 2 == 0))
	Console.Write(num + " ");
Console.WriteLine();
	
myInts.Sort((x, y) => y.CompareTo(x));	// Will change the ordering in the original collection
foreach (int num in myInts)
	Console.Write(num + " ");
Console.WriteLine();

myInts.OrderBy(x => x).Dump(); // Only changes the order of elements in a new temporary collection
foreach (int num in myInts)
	Console.Write(num + " ");
Console.WriteLine();





