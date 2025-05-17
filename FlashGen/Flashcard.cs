namespace FlashGen;

public sealed class Flashcard
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;

    public static List<Flashcard> Parse(string input)
    {
        var flashcards = new List<Flashcard>();
        var lines = input.Split(
            ['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        string currentQuestion = string.Empty;

        foreach (var line in lines)
        {
            var text = line.Trim().Replace("**", "").Trim();

            if (text.StartsWith("Q:", StringComparison.OrdinalIgnoreCase))
            {
                currentQuestion = text[2..].Trim();
                continue;
            }

            if (text.StartsWith("A:", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(currentQuestion))
            {
                var answer = text[2..].Trim();
                flashcards.Add(new Flashcard
                {
                    Question = currentQuestion,
                    Answer = answer
                });

                currentQuestion = string.Empty;
            }
        }

        return flashcards;
    }
}