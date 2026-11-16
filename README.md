# Student Course API

An ASP.NET core minimal API (C#, .NET 10) for managing courses, with simple browser page that lists & searches courses using `fetch()`.  Data is stored in memory & resets when server restarts. 

**Author:** Cam Mariotti

## Prerequisites
- .NET 10 SDK
- Any browser; `curl` for testing (built into Windows 10+)

## Run It
```bash
git clone https://github.com/CamMariotti627/StudentCourseApi.git
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

### Screenshots
| Test | Result |
|---|---|
| GET all (200) | ![GET all](screenshots/get-all.png) |
| GET by id (200) | ![GET by id](screenshots/get-by-id.png) |
| GET missing id (404) | ![404](screenshots/get-missing-404.png) |
| POST valid (201) | ![POST 201](screenshots/post-201.png) |
| POST invalid (400) | ![POST 400](screenshots/post-invalid-400.png) |
| PUT (200) | ![PUT](screenshots/put-200.png) |
| DELETE (204) | ![DELETE](screenshots/delete-204.png) |
| Browser page | ![Browser page](screenshots/browser-page.png) |
| Search (stretch goal) | ![Search](screenshots/search.png) |

## Stretch goal
Search by code, title, or instructor via `GET /api/courses?q=text`, also available from the search box on the web page.


## Reflection
C# is surprisingly similar to Java, and it was very interesting to learn about the flexible functionalities available in the language and how they work together.  Asynchronous functionality was what I found to be most interesting.  The way that it allows other functions to continue while waiting for a response from the server is incredibly useful.  Overall Object Oriented Programming seems to make more sense to me than the Scripting languages I had learned previously. 

Some issues that I debugged in this program arose after I had finished the bare requirements and began to change the GET functionality to work with a search function, and return all courses when asked.  I had accidentally placed p as the parameter while the code used q. I also accidentally commented out the `return course is null` line while adding notes which left `? Results.NotFound()` with nothing before it, so it wouldn't compile.  These fixes were pretty easy, for the first I replaced p with q, and for the second I read through the notes code and removed the `//` before the required `return` statement so the program could compile.  Each was tested through PowerShell with curl.

Finally, the API connects to the browser page with an event listener for the button click that causes the `fetch` to send a GET request using `await`, so the program will continue processing while the `MapGet` returns JSON, and JavaScript builds the table to display the results returned by that request. 