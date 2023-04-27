# GraphQL Interface Generator

This program is a code generator that generates C# code from a GraphQL API schema. It uses the GraphQL Generator library to retrieve the schema and generate the C# code. The generated code includes data classes that correspond to the GraphQL schema types, and a client class that can be used to make GraphQL queries and mutations against the API.

This program includes a method to extract the data classes from the generated code and convert them into interfaces. The generated interfaces can be used to implement the repository pattern in a .NET Core application.

## Usage

To use this program, you need to provide the following command line arguments:

* --file (-f): The file path to save the generated code. Default is ..\Infrastructure\Common\Interfaces\WFM\GqlApiClient.cs.
* --region (-r): The name of the region in the generated code that contains the data classes. Default is data classes.
* --uri (-u): The URI of the GraphQL API endpoint. Default is https://wfm-mtcloud.ptxcloud.com:3005/graphql.
To run the program, open a command prompt and navigate to the directory containing the executable file. Then, type the following command:
  
GqlApiClientGenerator.exe --file <file_path> --region <region_name> --uri <api_uri>  
  
For example:  
  `GqlApiClientGenerator.exe --file "..\MyProject\Repositories\GqlApiClient.cs" --region "entities" --uri "https://myapi.com/graphql"`

## How to Run  

To run the program from the command line, navigate to the directory containing the executable file and type the command with the desired command line arguments (as described in the Usage section above).

To run the program from Visual Studio, open the solution file and build the project. Then, open the command window (View > Other Windows > Command Window) and type the command with the desired command line arguments (as described in the Usage section above). You can also set the command line arguments in the project properties (Debug > Application arguments) and run the program with the Start button.

## Building

To build the GraphQL Interface Generator from source, you will need to have .NET Core 3.1 or later installed. Once you have cloned the repository, navigate to the root directory and enter the following command:

