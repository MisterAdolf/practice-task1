using System;
using System.Collections.Generic;
using System.IO;

class DictionaryApp
{
    private string type;
    private Dictionary<string, List<string>> data;
    private string filename;

    public DictionaryApp(string type)
    {
        this.type = type;
        this.filename = type + ".txt";
        this.data = new Dictionary<string, List<string>>();
        Load();
    }

    public void AddWord(string word, string translation)
    {
        if (!data.ContainsKey(word))
            data[word] = new List<string>();
        data[word].Add(translation);
    }

    public void ReplaceWord(string oldWord, string newWord)
    {
        if (data.ContainsKey(oldWord))
        {
            data[newWord] = data[oldWord];
            data.Remove(oldWord);
        }
    }

    public void ReplaceTranslation(string word, string oldTranslation, string newTranslation)
    {
        if (data.ContainsKey(word))
        {
            var translations = data[word];
            int index = translations.IndexOf(oldTranslation);
            if (index != -1)
                translations[index] = newTranslation;
        }
    }

    public void DeleteWord(string word)
    {
        data.Remove(word);
    }

    public void DeleteTranslation(string word, string translation)
    {
        if (data.ContainsKey(word))
        {
            var translations = data[word];
            if (translations.Count > 1)
                translations.Remove(translation);
        }
    }

    public void Search(string word)
    {
        if (data.ContainsKey(word))
        {
            Console.WriteLine($"Переводы слова '{word}': {string.Join(", ", data[word])}");
        }
        else
        {
            Console.WriteLine("Слово не найдено.");
        }
    }

    public void ExportWord(string word, string exportFile)
    {
        if (data.ContainsKey(word))
        {
            File.WriteAllText(exportFile, word + " - " + string.Join(", ", data[word]));
        }
    }

    public void Save()
    {
        using StreamWriter writer = new StreamWriter(filename);
        foreach (var pair in data)
        {
            writer.WriteLine(pair.Key + ":" + string.Join(",", pair.Value));
        }
    }

    public void Load()
    {
        if (File.Exists(filename))
        {
            foreach (var line in File.ReadAllLines(filename))
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string word = parts[0];
                    var translations = parts[1].Split(',');
                    data[word] = new List<string>(translations);
                }
            }
        }
    }

    public static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine("\n1. Создать/открыть словарь\n2. Выход\nВаш выбор: ");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.Write("Введите тип словаря (например, англо-русский): ");
                string dictType = Console.ReadLine();
                DictionaryApp dict = new DictionaryApp(dictType);
                DictionaryMenu(dict);
            }
            else if (choice == "2")
                break;
        }
    }

    public static void DictionaryMenu(DictionaryApp dict)
    {
        while (true)
        {
            Console.WriteLine("\n1. Добавить слово\n2. Заменить слово\n3. Заменить перевод\n4. Удалить слово\n5. Удалить перевод\n6. Найти перевод\n7. Экспорт слова\n8. Сохранить и выйти в меню\nВаш выбор: ");
            string choice = Console.ReadLine();
            if (choice == "8") { dict.Save(); break; }

            Console.Write("Введите слово: ");
            string word = Console.ReadLine();
            string trans, newWord, newTrans, file;

            switch (choice)
            {
                case "1":
                    Console.Write("Введите перевод: ");
                    trans = Console.ReadLine();
                    dict.AddWord(word, trans);
                    break;
                case "2":
                    Console.Write("Новое слово: ");
                    newWord = Console.ReadLine();
                    dict.ReplaceWord(word, newWord);
                    break;
                case "3":
                    Console.Write("Старый перевод: ");
                    trans = Console.ReadLine();
                    Console.Write("Новый перевод: ");
                    newTrans = Console.ReadLine();
                    dict.ReplaceTranslation(word, trans, newTrans);
                    break;
                case "4":
                    dict.DeleteWord(word);
                    break;
                case "5":
                    Console.Write("Перевод для удаления: ");
                    trans = Console.ReadLine();
                    dict.DeleteTranslation(word, trans);
                    break;
                case "6":
                    dict.Search(word);
                    break;
                case "7":
                    Console.Write("Имя файла для экспорта: ");
                    file = Console.ReadLine();
                    dict.ExportWord(word, file);
                    break;
            }
        }
    }

    static void Main(string[] args)
    {
        MainMenu();
    }
}