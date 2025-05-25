using System.Text;

namespace FlashGenDesktop;

public sealed class Flashcard
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;

    public static List<Flashcard> Parse(string input)
    {
        var flashcards = new List<Flashcard>();
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        string currentQuestion = string.Empty;
        StringBuilder currentAnswer = new();

        foreach (var line in lines)
        {
            var text = line.Trim()
                .Replace("**", "")
                .Replace("•", "")
                .Trim();

            // Start new question
            if (text.StartsWith("Q:", StringComparison.OrdinalIgnoreCase) ||
                text.StartsWith("Question:", StringComparison.OrdinalIgnoreCase))
            {
                // If we have an unfinished flashcard, add it
                if (!string.IsNullOrEmpty(currentQuestion) && currentAnswer.Length > 0)
                {
                    flashcards.Add(new Flashcard
                    {
                        Question = currentQuestion,
                        Answer = currentAnswer.ToString().Trim()
                    });
                    currentAnswer.Clear();
                }

                currentQuestion = text.Contains(':')
                    ? text.Split(':', 2)[1].Trim()
                    : string.Empty;

                continue;
            }

            // Start answer
            if ((text.StartsWith("A:", StringComparison.OrdinalIgnoreCase) ||
                 text.StartsWith("Answer:", StringComparison.OrdinalIgnoreCase))
                && !string.IsNullOrWhiteSpace(currentQuestion))
            {
                currentAnswer.Clear();
                currentAnswer.Append(text.Split(':', 2)[1].Trim());
                continue;
            }

            // If we're already parsing an answer, collect additional lines
            if (!string.IsNullOrWhiteSpace(currentQuestion) && currentAnswer.Length > 0)
            {
                currentAnswer.AppendLine();
                currentAnswer.Append(text);
            }
        }

        // Add last flashcard if present
        if (!string.IsNullOrEmpty(currentQuestion) && currentAnswer.Length > 0)
        {
            flashcards.Add(new Flashcard
            {
                Question = currentQuestion,
                Answer = currentAnswer.ToString().Trim()
            });
        }

        return flashcards;
    }

}