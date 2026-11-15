var builder = WebApplication.CreateBuilder(args);  
//*always - sets up web app settings/tools
var app = builder.Build();
//*always - builds app - with web server

app.UseDefaultFiles();  // loads wwwroot/index - must be 1st
app.UseStaticFiles();   // lets server send files from wwwroot/index.html
// *always used when serving a page


// starter data (resets when server restarts)
var courses = new List<Course>
{
    new(1, "ART101", "Intro Art History", 3, "Dr. Smith"), 
    new(2, "ART132", "World Art", 3, "Dr. Smith"),         
    new(3, "MATH180", "Algebra", 3, "Dr. Robins"),      
    new(4, "MUSC201", "Orchestra", 3, "Dr. Miller"),
    new(5, "MATH173", "Statistics", 3, "Dr. Hawkins")
//  new(id, code, title, credit amt, instructor) pattern for new data

};

// GET all
app.MapGet("/api/courses", () => Results.Ok(courses));
// ^^^^ recurring framework:
// GET /api/courses -> 200 OK + every course as JSON

// GET 
app.MapGet("/api/courses/{id:int}", (int id) =>
// ^^^^
// *recurring framework^^ anatomy below:
// app.MapVerb("path", () => Results.Status(data)); 
// path & data change depedent on HTTP verb (GET/POST/PUT/DELETE)
{
    var course = courses.FirstOrDefault(c => c.Id == id);
// *recurring framework- find 1st course w matching id, null if none
    return course is null
// is it missing?
        ? Results.NotFound()  // yes-> 404 not found
        : Results.Ok(course); // no -> 200 OK + course as JSON
});                           // end of route

// POST /api/courses -> add course (201) or reject bad data (400)
app.MapPost("/api/courses", (CourseInput input) =>
//* recurring  framework: record parameter on POST - read from JSON body
{
    string? error = ValidateCourse(input);
    if (error is not null)
        return Results.BadRequest(error);
    
    int newId = courses.Count == 0 ? 1 : courses.Max(c => c.Id) + 1;
    var course = new Course(newId, input.Code, input.Title, input.Credits, input.Instructor);
    courses.Add(course);
    return Results.Created($"/api/courses/{newId}", course);
});

// PUT /api/courses/{id:int}" -> update (200), missing 404, bad data 400

app.MapPut("/api/courses/{id:int}", (int id, CourseInput input) =>
{
    int index = courses.FindIndex(c => c.Id == id);
    if (index == -1)
        return Results.NotFound();
    
    string? error = ValidateCourse(input);
    if (error is not null)
        return Results.BadRequest(error);
    
    var updated = new Course(id, input.Code, input.Title, input.Credits, input.Instructor);
    courses[index] = updated;
    return Results.Ok(updated);
});

// DELETE /api/courses/{id} -> removed(204) missing (404)
app.MapDelete("/api/courses/{id:int}", (int id) =>
{
    var course = courses.FirstOrDefault(c => c.Id == id);
    if (course is null)
        return Results.NotFound();

    courses.Remove(course);
    return Results.NoContent();
});



app.Run();
//^^ always last line of code
// starts server
// [Ctrl + C] to stop

// validates input runs through if conditionals to fit data reqs
static string? ValidateCourse(CourseInput input)
{
    if (string.IsNullOrWhiteSpace(input.Code))
        return "Code is required.";
    if (string.IsNullOrWhiteSpace(input.Title))
        return "Title is required.";
    if (string.IsNullOrWhiteSpace(input.Instructor))
        return "Instructor is required.";
    if (input.Credits < 1 || input.Credits > 6)
        return "Credits must be between 1 and 6.";
    return null;
}



record Course(int Id, string Code, string Title, int Credits, string Instructor);
// required model: one line makes whole data type
record CourseInput(string Code, string Title, int Credits, string Instructor);
// what users send to add/edit a course - server assigns ID so omit

