# GraphQL Interface Generator

The GraphQL Interface Generator is a tool that automatically generates C# interfaces from a GraphQL API. The tool reads the schema of a GraphQL endpoint and generates C# code for each of the API's data classes as interfaces. This can be useful for defining contracts between services or for generating client-side code that depends on the API.

## Usage

The GraphQL Interface Generator is a .NET console application that can be run from the command line. It takes the following parameters:

Usage: graphqlgen [options]

Options:
-f, --file <FILE> The file path to generate the code. (default: ../Infrastructure/Common/Interfaces/WFM/GqlApiClient.cs)
-r, --region <NAME> The region name to extract classes from. (default: data classes)
-u, --uri <URI> The GraphQL API endpoint URI. (default: https://wfm-mtcloud.ptxcloud.com:3005/graphql)
--help Display this help screen.
--version Display version information.


To run the tool, navigate to the directory containing the graphqlgen.exe file and enter the following command:

graphqlgen --uri <GRAPHQL_ENDPOINT_URI> --file <OUTPUT_FILE_PATH> --region <REGION_NAME>
  
  
Replace `<GRAPHQL_ENDPOINT_URI>` with the URL of the GraphQL API endpoint, `<OUTPUT_FILE_PATH>` with the path and file name of the output C# file, and `<REGION_NAME>` with the name of the C# region containing the data classes you want to generate interfaces for.

## Building

To build the GraphQL Interface Generator from source, you will need to have .NET Core 3.1 or later installed. Once you have cloned the repository, navigate to the root directory and enter the following command:

