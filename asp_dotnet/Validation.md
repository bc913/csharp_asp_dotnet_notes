# Validation

## Definition of Validation Rules
There are several ways to achieve this.

- Data annotations
- IValidatable Object
- Custom attribute derives from `ValidationAttribute`.
- FluentValidation

Rules:
1. Validate only input, not ouput.

## Checking Validation Rules
ModelState

## Reporting Validation Errors
Response status should be 422 - Unprocessable Entity and response body should contain validation errors.

