__Description:__
This project is a C# Windows Forms application that interacts with a RESTful API to perform CRUD operations on a collection of books. It uses RestSharp for making API requests and includes token-based authentication, form validation, and response handling.

__Features__
Fetch book details using GET requests.
Create new books via POST requests.
Update book information through PUT requests.
Delete books using DELETE requests.
Handles token authentication for secure API calls.
__Usage__
Launch the application.
Use the input fields to add, update, or delete books.
Fetch all books or a specific book by its ID.
Ensure you have your access token set up for authenticated API requests.
__API Endpoints__
GET /books – Retrieve all books.
GET /books/{id} – Fetch a specific book by ID.
POST /books – Add a new book.
PUT /books/{id} – Update an existing book.
DELETE /books/{id} – Delete a book.
__Requirements__
Visual Studio 2019 or later.
.NET Framework 4.7.2 or higher.
RestSharp library installed via NuGet.
__Contributing__
Feel free to fork this repository and submit pull requests. All contributions are welcome!

