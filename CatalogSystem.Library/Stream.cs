﻿using System;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace CatalogSystem.Library
{
    public sealed class Stream
    {
        private static Stream Instance = new Stream();

        public static string path = $"{Directory.GetCurrentDirectory()}\\catalog.csv";

        public static bool FileExists()
        {
            return File.Exists(path);
        }

        public static Stream GetInstance()
        {
            return Instance;
        }

        public static void ReadCatalogFile(Catalog libraryCatalog, ref int counter)
        {

            if (FileExists())
            {
                using (TextFieldParser csvParser = new TextFieldParser(path))
                {
                    csvParser.SetDelimiters(new string[] {","});
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    while (!csvParser.EndOfData)
                    {
                        Book book;
                        DateTime? dueDate;
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();
                        int id = int.Parse(fields[0]);
                        string title = fields[1];
                        string author = fields[2];
                        try
                        {
                            dueDate = DateTime.Parse(fields[4]);
                        }
                        catch (FormatException)
                        {
                            dueDate = null;
                        }

                        bool checkedOut = bool.Parse(fields[3].ToLower());

                        counter++;
                        if (dueDate != null)
                        {
                            book = new Book(title, author, counter, checkedOut, dueDate);
                        }
                        else
                        {
                            book = new Book(title, author, counter, checkedOut);
                        }


                        libraryCatalog.libraryCatalog.Add(book);
                    }
                }
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }

        public static void WriteCatalogToFile(Catalog libraryCatalog)
        {
            if (FileExists())
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (var book in libraryCatalog.libraryCatalog)
                    {
                        var bookId = book.GetBookId();
                        var title = book.GetTitle();
                        var author = book.GetAuthor();
                        var checkedOut = book.GetCheckedOut();
                        var dueDate = book.GetDueDate();

                        writer.WriteLine($"{bookId},{title},{author},{checkedOut},{dueDate}");

                        writer.Flush();

                    }
                }
            }
        }
    }
}