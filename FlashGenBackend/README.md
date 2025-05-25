# FlashGenBackend – FastAPI AI Flashcard Generator (Backend)

This is the backend service for **FlashGen**, a desktop app that converts raw text into flashcards using Cohere's AI API. The backend accepts POST requests with text + optional instructions and returns formatted Q&A flashcards.

---

## 🚀 Getting Started

### ✅ 1. Requirements

- Python 3.8+
- pip

---

### ✅ 2. Setup Instructions

**Clone the repo and navigate to the backend folder:**

```bash
cd FlashGenBackend
```

**Create a virtual environment:**

```bash
python -m venv venv
venv\Scripts\activate          # Windows
# OR
source venv/bin/activate       # Mac/Linux
```

**Install dependencies:**

```bash
pip install -r requirements.txt
```

**Create a `.env` file with your Cohere API key:**

```
COHERE_API_KEY=your-cohere-api-key-here
```

---

### ✅ 3. Run the Server Locally

```bash
uvicorn main:app --reload
```

The server will start at:

```
http://localhost:8000
```
---

## 📨 API Endpoint

### `POST /generate`

**Payload Example:**

```json
{
  "text": "Photosynthesis is the process...",
  "prompt": "convert answers to lowercase"
}
```

**Returns:**

```json
{
  "result": "Question: What is photosynthesis?\nAnswer: it's the process by which..."
}
```

---

## 🧠 Prompt Template (Backend Logic)

The backend wraps the user input with this format:

```
Convert the following text into flashcards in Q&A format.
Each flashcard should look like:
Q: [question]
A: [answer]

Additional instruction: [your custom prompt]

Text:
[your text here]
```

---

## 🧩 Notes

- FlashGenDesktop parses the returned text using simple `Q:` / `A:` or `Question:` / `Answer:` formats.
- Ensure that the output from the AI is consistent with that format for reliable parsing.
---
