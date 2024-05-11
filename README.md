# NUTRIAPP

## Team

- Danny Gramowski, djg5455@rit.edu
- Austyn Wright, arw5395@rit.edu
- Raynard Miot, rhm8082@rit.edu
- Dan Donchuk, dod4030@rit.edu
- Jack Lindsey-Noble, jgl2651@rit.edu

### Instructions to run the app:

```bash
# Clone the repository
git clone https://github.com/austyn06/NUTRIAPP.git
cd NUTRIAPP

# Install frontend dependencies
cd frontend/nutriapp
npm install

# Install backend dependencies
cd ../../backend
dotnet restore

# Run the frontend
cd ../frontend/nutriapp
npm run dev

# Run the backend
cd ../../backend
dotnet run
```

### Testing the app:

```bash
# Backend tests
cd backend
dotnet test
```