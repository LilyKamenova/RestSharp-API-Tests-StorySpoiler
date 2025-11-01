🧪 Story Spoiler API Tests (RestSharp + NUnit)

Automated backend API tests for the Story Spoiler system, developed as part of a QA Automation exam.
This project demonstrates practical REST API testing using RestSharp and NUnit, covering the main CRUD operations and negative test scenarios.

🚀 Overview

Story Spoiler is an online platform for sharing and managing story spoilers.
The tests verify the main API endpoints responsible for creating, editing, retrieving, and deleting stories.

API Base URL:
👉 https://d3s5nxhwblsjbi.cloudfront.net/api


🧩 Tech Stack

C# / .NET 8.0

NUnit – test framework

RestSharp – API requests

JSON serialization

🧠 Implemented Test Scenarios
#	Test Case	Endpoint	Expected Result
1	Create Story	POST /api/Story/Create	201 Created + “Successfully created!”
2	Edit Story	PUT /api/Story/Edit/{id}	200 OK + “Successfully edited”
3	Get All Stories	GET /api/Story/All	200 OK + Non-empty array
4	Delete Story	DELETE /api/Story/Delete/{id}	200 OK + “Deleted successfully!”
5	Create Story (Missing Fields)	POST /api/Story/Create	400 Bad Request
6	Edit Non-existing Story	PUT /api/Story/Edit/{invalidId}	404 Not Found
7	Delete Non-existing Story	DELETE /api/Story/Delete/{invalidId}	400 Bad Request

```
🧱 Project Structure
RestSharp-API-Tests-StorySpoiler/
├── StorySpoilerTests/
│   ├── StorySpoilerTests.cs
│   ├── Using.cs
│   ├── DTOs/
│   │   ├── ApiResponseDTO.cs
│   │   └── StoryDTO.cs
│   
├── Documentation/
│   ├── Exam_Assignment.md
│   
├── Screenshots/
│   ├── test_results.png
│
|── StorySpoiler.sln   
└── README.md
```
🧾 Exam Assignment

📄 Full exam description available in Documentation/Exam_Assignment.md


