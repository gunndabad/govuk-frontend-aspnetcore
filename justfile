set shell := ["pwsh", "-nop", "-c"]

default:
  @just --list

# Install Playwright
install-playwright:
  @dotnet build tests/GovUk.Frontend.AspNetCore.IntegrationTests -c Release -v quiet --nologo
  @./tests/GovUk.Frontend.AspNetCore.IntegrationTests/bin/Release/net8.0/playwright.ps1 install chromium

# Restore dependencies
restore *ARGS:
  @dotnet restore {{ARGS}}

# Build the solution
build *ARGS:
  @dotnet build {{ARGS}}

# Run the tests
test *ARGS: (unit-tests ARGS) (conformance-tests ARGS) (integration-tests ARGS)

# Run the unit tests
unit-tests *ARGS:
  @dotnet test tests/GovUk.Frontend.AspNetCore.Tests/ {{ARGS}}

# Run the conformance tests
conformance-tests *ARGS:
  @dotnet test tests/GovUk.Frontend.AspNetCore.ConformanceTests/ {{ARGS}}

# Run the integration tests
integration-tests *ARGS:
  @dotnet test tests/GovUk.Frontend.AspNetCore.IntegrationTests/ {{ARGS}}

# Format the C# code
format *ARGS:
  @dotnet format {{ARGS}}

# Package the library
pack *ARGS:
  @dotnet pack src/GovUk.Frontend.AspNetCore/ {{ARGS}}
