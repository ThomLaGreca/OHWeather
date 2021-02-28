# OHWeather

## Application Use
Orinigally I had planned to use a dialog to allow the user to select a file from a directory however the integration of Windows forms seemed to be causing some issues. For the sake of time and scope this avenue was abandoned. In order for the application to read a selected file it must reside in the {root}/FileBucket folder. If the application is being run from Visual Studio add the files to the FileBucket folder and set the file property 'Copy to output directory' to always/if newer. I have included a few sample files in the project already, including the BOM 'All years' test data for your convenience. 

## Structure
The structure of the application is seperated to some degree into its different concerns via each of the seperate projcts. 
In practise the application could have been structured into one main project with a testing project. However, my goal was to excersie a clear and easy to extend seperation.

## Json Sample vs Result
In the sample output the data was displayed as a single object with single children. 
Data > year > month. 
In the description it was stated that the end result would actually be a collection of collections. My result uses this ideal to display the results.

## System.Text.Json
As you will see there are two extra classes added to the OHWeather.Data.Model project. Namely WeatherDataRoot.cs and WeatherDataForMonthRoot.cs. 
The .NET Core JSON serialiser was built to intentionally exclude the TypeNameHandling setting that Newtonsoft.Json uses for example. The main reason stated by the .NET teams is that this was done for security reasons.
As such, these classes were used to circumvent the lack of the root object name as depicted in descriptions Sample Output.
