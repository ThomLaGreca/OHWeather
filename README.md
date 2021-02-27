# OHWeather

## Structure
The structure of the application is seperated to some degree into its different concerns via each of the seperate projcts. 
In practise the application could have been structured into one main project with a testing project. However, my goal was to excersie a clear and easy to extend seperation.
The SOLID principles could have indeed been actioned further, using dependency injection for example but this seemed to go a little overboard considering time and scope. 

## Json Sample vs Result
In the sample output the data was displayed as a single object with single children. 
Data > year > month. 
In the description it was stated that the end result would actually be a collection of collections. My result uses this ideal to display the results.

## System.Text.Json
As you will see there are two extra classes added to the OHWeather.Data.Model project. Namely WeatherDataRoot.cs and WeatherDataForMonthRoot.cs. 
The .NET Core JSON serialiser was built to intentionally exclude the TypeNameHandling setting that Newtonsoft.Json uses for example. The main reason stated by the .NET teams is that this was done for security reasons.
As such, these classes were used to circumvent the lack of the root object name as depicted in descriptions Sample Output.
