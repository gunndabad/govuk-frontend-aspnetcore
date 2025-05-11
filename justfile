set shell := ["pwsh", "-nop", "-c"]

default:
  @just --list

# Restore dependencies
restore *ARGS:
  @dotnet restore {{ARGS}}

# Build the solution
build *ARGS:
  @dotnet build {{ARGS}}

# Run the tests
test *ARGS:
  @dotnet test {{ARGS}}

# Format the C# code
format *ARGS:
  @dotnet format {{ARGS}}
