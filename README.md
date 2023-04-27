GraphQL Code Generator
This is a command-line tool that generates C# code for a GraphQL API. The generated code includes a full client for the API and interfaces for the data classes used by the client.

Installation
To use this tool, you'll need to have .NET Core 3.1 or later installed on your machine. You can download it from Microsoft's website.

To install the tool, run the following command:

dotnet tool install --global GraphQLCodeGenerator

This will install the tool globally on your machine.

Usage
To generate code for a GraphQL API, you'll need to provide the tool with the following information:

GraphQlApiClientCodeGenerator.exe [-u <uri>] [-f <file>] [-r <region>]

The optional arguments are as follows:

-u, --uri: The URI of the GraphQL API endpoint. Defaults to https://wfm-mtcloud.ptxcloud.com:3005/graphql.
-f, --file: The file path to generate the code. Defaults to ..\Infrastructure\Common\Interfaces\WFM\GqlApiClient.cs.
-r, --region: The name of the region in the code containing the data classes to extract. Defaults to data classes.

How to Pass Arguments
To pass arguments to the GraphQlApiClientCodeGenerator, simply include them after the executable name. For example:

GraphQlApiClientCodeGenerator.exe -u https://my.graphql.api -f MyClass.cs -r MyRegion

The URI of the GraphQL endpoint
The file path where the generated code should be saved
The name of the region in the code where the data classes are defined

How to Run
To run the GraphQlApiClientCodeGenerator, navigate to the directory containing the executable and run the following command:

License
This tool is released under the MIT License.
