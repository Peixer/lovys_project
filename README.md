# Lovys Project

Lovys Tech Challenge - API for calendar interview - [Documentation](https://lovys.azurewebsites.net/swagger/index.html)

The purpose of the test was to create an API for calendar interview, where both the candidate and the interviewer would have to register their availabilities, and only 1-hour period with the am|pm time format are accepted. It will be possible to obtain the available times for interviews of a candidate with one or more interviewers.

- I choose to create two projects Lovys.WebApp and Lovys.Core, each with his test project.
- Lovys.WebApp 
  - Controllers using Rest API naming patterns
  - Validation using FluentValidation, Regex to validate availability time input
  - JWT Authentication
  - Swagger documentation
  - Entity Framework in-memory database
   
- Lovys.WebApp.Test
  - Test classes for all controllers
  - Moq, Nunit e Shouldly 

- Lovys.Core 
  - Folder pattern per module (in this case, there is only the Calendar module)
  - Use of Repository, Service, Entities, DTO patterns
  - Customization in NewtonsoftJson serialization
   
- Lovys.Core.Test
  - Moq, Nunit e Shouldly 


- Deploy
  - Azure Cloud - App Service
