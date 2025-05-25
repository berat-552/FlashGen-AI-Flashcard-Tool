# FlashGenDesktop – WPF Flashcard Generator (Frontend)

This is the WPF frontend for **FlashGen**, a desktop app that converts raw text files into AI-generated flashcards using a backend powered by FastAPI and Cohere.

---

## 🚀 Getting Started

### ✅ 1. Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Visual Studio 2022 or later](https://visualstudio.microsoft.com/)
- Backend running locally (see `FlashGenBackend`)

---

### ✅ 2. Run the Desktop App (WPF)

**Steps:**

```bash
1. Open the `FlashGenDesktop/` folder in Visual Studio.
2. Set build configuration to `Debug` or `Release`.
3. Press `F5` or click **"Start"** to run the app.
```

---

### ✅ 3. How to Use

1. Launch the app.
2. Drag and drop a `.txt` file into the drop zone.
3. (Optional) Add a prompt to guide flashcard formatting (e.g., `make answers lowercase`).
4. Click **Generate Flashcards**.
5. View the generated flashcards in the list view.

---

## 🧪 Example Prompts

- use simple language
- convert answers to lowercase

---

## 🧩 Notes

- The app sends text and prompt input to a backend service via POST (`http://localhost:8000/generate`)
- Flashcards are parsed from the response in the format:

```bash
Question: ...
Answer: ...
```
