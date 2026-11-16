# Student Course API

And ASP.NET core minimal API (C#, .NET 10) for managing courses, with simple browser page that lists & searches courses using `fetch()`.  Data is stored in memory & resets when server restarts. 

**Author:** Cam Mariotti

## Prerequisites
- .NET 10 SDK
- .Any browser; `curl` for testing (built into Windows 10+)

## Run It
```bash
git clone https://github.com/YOUR-USERNAME/StudentCourseApi.git
cd StudentCourseApi
dotnet run --urls http://localhost:5050
```

Then open http://localhost:5050/ for the course page

## API routes:
| Method | Route | Success | Errors |
|---|---|---|---|
| GET | `/api/courses` | 200: all courses | |
| GET | `/api/courses?q=text` | 200: courses whose code, title or instructor contains the text | |
| GET | `/api/courses/{id}` | 200: one course | 404 if not found |
| POST | `/api/courses` | 201: created course | 400 if invalid |
| PUT | `/api/courses/{id}` | 200: updated course | 404 if not found, 400 if invalid |
| DELETE | `/api/courses/{id}` | 204 | 404 if not found |

**Validation (400):** code, title, and instructor cannot be blank; credits must be between 1-6.

## Sample JSON (POST / PUT body)
```json
{
    "code": "CMPS360",
    "title": "Web Programming",
    "credits": 3,
    "instructor": "Professor Smith"
}
```

## Testing
Every route was tested with `curl.exe -i` in Powershell to show the status code, using JSON test files located in the `tests/` folder, and web page was tested in browser. 

```powershell
curl.exe -i http://localhost:5050/api/courses
curl.exe -i http://localhost:5050/api/courses/1
curl.exe -i http://localhost:5050/api/courses/99
curl.exe -i -X POST http://localhost:5050/api/courses -H "Content-Type: application/json" --data "@tests/valid-course.json"
curl.exe -i -X POST http://localhost:5050/api/courses -H "Content-Type: application/json" --data "@tests/invalid-course.json"
curl.exe -i -X PUT http://localhost:5050/api/courses/1 -H "Content-Type: application/json" --data "@tests/update-course.json"
curl.exe -i -X DELETE http://localhost:5050/api/courses/2
curl.exe -i "http://localhost:5050/api/courses?q=smith"
```