
using System;
using System.Collections.Generic;

// Интерфейс для клонирования
public interface IPrototype
{
    IPrototype Clone();
}

// Класс, представляющий раздел документа
public class Section : IPrototype
{
    public string Title { get; set; }
    public string Content { get; set; }

    public Section(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public IPrototype Clone()
    {
        return new Section(Title, Content);
    }

    public override string ToString()
    {
        return $"Раздел: {Title}, Содержание: {Content}";
    }
}

// Класс, представляющий изображение документа
public class Image : IPrototype
{
    public string URL { get; set; }

    public Image(string url)
    {
        URL = url;
    }

    public IPrototype Clone()
    {
        return new Image(URL);
    }

    public override string ToString()
    {
        return $"Изображение: {URL}";
    }
}

// Класс, представляющий основной документ
public class Document : IPrototype
{
    public string Title { get; set; }
    public string Content { get; set; }
    public List<Section> Sections { get; set; } = new List<Section>();
    public List<Image> Images { get; set; } = new List<Image>();

    public Document(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public IPrototype Clone()
    {
        // Клонируем основной документ
        Document clonedDocument = (Document)this.MemberwiseClone();
        // Клонируем вложенные элементы (разделы и изображения)
        clonedDocument.Sections = new List<Section>();
        foreach (var section in Sections)
        {
            clonedDocument.Sections.Add((Section)section.Clone());
        }
        clonedDocument.Images = new List<Image>();
        foreach (var image in Images)
        {
            clonedDocument.Images.Add((Image)image.Clone());
        }
        return clonedDocument;
    }

    public override string ToString()
    {
        string sectionsInfo = string.Join("\n", Sections);
        string imagesInfo = string.Join("\n", Images);
        return $"Документ: {Title}, Содержание: {Content}\nРазделы:\n{sectionsInfo}\nИзображения:\n{imagesInfo}";
    }
}

// Класс для управления документами
public class DocumentManager
{
    private IPrototype _prototype;

    public DocumentManager(IPrototype prototype)
    {
        _prototype = prototype;
    }

    public IPrototype CreateDocument()
    {
        return _prototype.Clone();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем основной документ
        Document originalDocument = new Document("Основной документ", "Это основной документ");
        originalDocument.Sections.Add(new Section("Раздел 1", "Текст раздела 1"));
        originalDocument.Sections.Add(new Section("Раздел 2", "Текст раздела 2"));
        originalDocument.Images.Add(new Image("https://example.com/image1.jpg"));
        originalDocument.Images.Add(new Image("https://example.com/image2.jpg"));

        Console.WriteLine("Оригинальный документ:");
        Console.WriteLine(originalDocument);

        // Используем DocumentManager для клонирования документа
        DocumentManager manager = new DocumentManager(originalDocument);
        Document clonedDocument = (Document)manager.CreateDocument();

        // Изменяем клонированный документ
        clonedDocument.Title = "Клонированный документ";
        clonedDocument.Content = "Это клонированный документ";
        clonedDocument.Sections[0].Content = "Измененный текст раздела 1";

        Console.WriteLine("\nКлонированный документ:");
        Console.WriteLine(clonedDocument);

        // Оригинальный документ остается неизменным
        Console.WriteLine("\nОригинальный документ после клонирования (неизменен):");
        Console.WriteLine(originalDocument);
    }
}
