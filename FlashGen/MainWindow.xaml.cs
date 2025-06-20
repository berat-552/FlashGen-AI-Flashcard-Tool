﻿using FlashGenDesktop;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace FlashGen;

public partial class MainWindow : Window
{
    private readonly string _placeholderText =
        "e.g. Add: 'Use simple terms and short answers'";

    private string _droppedFilePath = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
        PromptInput.Text = _placeholderText;
        PromptInput.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
    }

    private async void OnGenerateFlashcardsClicked(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_droppedFilePath))
        {
            MessageBox.Show(
                "Please drop a .txt file first.",
                "No File",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        string fileContents = File.ReadAllText(_droppedFilePath).Trim();

        if (string.IsNullOrWhiteSpace(fileContents))
        {
            MessageBox.Show(
                "The selected file is empty.",
                "Empty File",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        string userPrompt = PromptInput.Text.Trim();
        var backendClient = new BackendClient();

        GenerateButton.IsEnabled = false;
        GenerateButton.Content = "Generating...";

        try
        {
            string aiResponseText =
                await backendClient
                    .GenerateFlashcardsAsync(
                    fileContents, userPrompt);

            var flashcards = Flashcard.Parse(aiResponseText);

            if (flashcards.Count == 0)
            {
                MessageBox.Show(
                    "No flashcards were generated.",
                    "Empty Result",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            FlashcardList.ItemsSource = flashcards;
            FlashcardPlaceholderText.Visibility = flashcards.Count > 0
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "An error occurred while contacting the backend:\n" + ex.Message,
                "API Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        GenerateButton.Content = "Generate and Save Flashcards";
        GenerateButton.IsEnabled = true;
    }

    private void OnDragOverDropZone(object sender, DragEventArgs e)
    {
        e.Handled = true;

        var files = e.Data.GetData(DataFormats.FileDrop) as string[];
        if (files == null || files.Length < 1)
        {
            e.Effects = DragDropEffects.None;
            return;
        }

        var droppedFile = new FileInfo(files[0]);

        e.Effects = droppedFile.IsTextFile()
            ? DragDropEffects.Copy
            : DragDropEffects.None;
    }

    private void OnFileDroppedIntoZone(object sender, DragEventArgs e)
    {
        var files = e.Data.GetData(DataFormats.FileDrop) as string[];
        if (files == null || files.Length < 1) return;

        var file = new FileInfo(files[0]);

        if (!file.IsTextFile())
        {
            MessageBox.Show(
                "Please drop a valid .txt file.",
                "Invalid File",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        _droppedFilePath = file.FullName;

        // ✅ Update UI
        DropZoneText.Text = $"✔ [{file.Name}] uploaded successfully";
        DropZoneText.Foreground = new SolidColorBrush(
            Color.FromRgb(34, 139, 34)); // green tick text
        DropZone.BorderBrush = new SolidColorBrush(
            Color.FromRgb(0, 128, 0));
    }

    private void PromptInput_GotFocus(object sender, RoutedEventArgs e)
    {
        if (PromptInput.Text != _placeholderText) return;

        PromptInput.Text = string.Empty;
        PromptInput.Foreground = new SolidColorBrush(Colors.Black);
    }

    private void PromptInput_LostFocus(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(PromptInput.Text)) return;

        PromptInput.Text = _placeholderText;
        PromptInput.Foreground = new SolidColorBrush(
            Color.FromRgb(136, 136, 136));
    }
}