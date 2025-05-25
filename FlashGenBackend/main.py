from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import cohere
import os
from dotenv import load_dotenv

load_dotenv()
app = FastAPI()

co = cohere.Client(os.getenv("COHERE_API_KEY"))


class FlashcardRequest(BaseModel):
    text: str
    prompt: str = ""  # Optional user instruction


@app.post("/generate")
def generate(data: FlashcardRequest):
    try:
        base_prompt = (
            "Convert the following text into flashcards in Q&A format.\n"
            "Each flashcard should look like:\n"
            "Q: [question]\n"
            "A: [answer]"
        )

        user_instruction = data.prompt.strip()
        if user_instruction:
            base_prompt += f"\n\nAdditional instruction: {user_instruction}"

        full_prompt = f"{base_prompt}\n\nText:\n{data.text.strip()}"
        print(full_prompt)
        response = co.generate(
            model="command",
            prompt=full_prompt,
            max_tokens=500,
            temperature=0.5
        )

        return {"result": response.generations[0].text.strip()}

    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
