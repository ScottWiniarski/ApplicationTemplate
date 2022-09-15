using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace ApplicationTemplate.Services;

/// <summary>
///     This concrete service and method only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public class FileService : IFileService
{
    private readonly ILogger<IFileService> _logger;
    private readonly IFileService _fileService;
    private string _fileName;

    // these should not be here
    private List<UInt64> _movieIds;
    private List<string> _movieTitles;
    private List<string> _movieGenres;

    #region constructors

    // Constructors instantiate new objects when called
    // default constructor for ExampleClass
    public FileService()
    {
    }

    // another constructor
    public FileService(int myInt)
    {
        Console.WriteLine($"Constructor value {myInt}");
    }

    // passing a string to a constructor
    public FileService(string myString)
    {
        Console.WriteLine($"constructor value {myString}");
    }

    #endregion constructors
    // this constructor is passing in a variable called logger, from ILogger, of IFileService as long as it matches the filetype or interface

    public FileService(ILogger<IFileService> logger, IFileService fileService)
    {
        
        _logger = logger;
        logger.LogInformation("Here is some information");
        // since this is a constructor, the file will be instantiated with every calling
        _fileName = $"{Environment.CurrentDirectory}/movies.csv";

        _movieIds = new List<UInt64>();
        _movieTitles = new List<string>();
        _movieGenres = new List<string>();

    }
    public void Read()
    {
        _logger.LogInformation("Reading");
        _logger.Log(LogLevel.Information, "Reading");
        Console.WriteLine("*** I am reading");

        //StreamReader sr = new StreamReader(_fileName);

        // create parallel lists of movie details
        // lists must be used since we do not know number of lines of data
       /* List<UInt64> MovieIds = new List<UInt64>(); 
        List<string> MovieTitles = new List<string>(); 
        List<string> MovieGenres = new List<string>(); 
        */            // to populate the lists with data, read from the data file
        try
        {
            StreamReader sr = new StreamReader(_fileName);
            // first line contains column headers
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                // first look for quote(") in string
                // this indicates a comma(,) in movie title
                int idx = line.IndexOf('"');
                if (idx == -1) //**If it's negative 1, it cannot find a comma in the string**
                            {
                   // no quote = no comma in movie title
                   // movie details are separated with comma(,)
                   string[] movieDetails = line.Split(',');
                   // 1st array element contains movie id
                   _movieIds.Add(UInt64.Parse(movieDetails[0])); //**MovieID was created earlier, adding string to ints with each loop
                   // 2nd array element contains movie title
                   _movieTitles.Add(movieDetails[1]);
                   // 3rd array element contains movie genre(s)
                   // replace "|" with ", "
                   _movieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else //****There is a quote in the movie title * ****
                            {
                    // quote = comma in movie title
                    // extract the movieId
                    _movieIds.Add(UInt64.Parse(line.Substring(0, idx - 1)));
                    // remove movieId and first quote from string
                    line = line.Substring(idx + 1);
                    // find the next quote
                    idx = line.IndexOf('"');
                    // extract the movieTitle
                    _movieTitles.Add(line.Substring(0, idx));
                    // remove title and last comma from the string
                    line = line.Substring(idx + 2);
                    // replace the "|" with ", "
                    _movieGenres.Add(line.Replace("|", ", "));
                }
            }
            // close file when done
            sr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Movies in file {Count}", _movieIds.Count);
    }

    public void Write(ulong movieId, string movieTitle, string genresString)
    {
        _logger.Log(LogLevel.Information, "Writing");
        Console.WriteLine("*** I am writing");

  

        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine($"{movieId},{movieTitle},{genresString}");
        sw.Close();
        // add movie details to Lists
        _movieIds.Add((ulong)movieId);
        _movieTitles.Add(movieTitle);
        _movieGenres.Add(genresString);
        // log transaction
        _logger.LogInformation("Movie id {Id} added", movieId);
    }

    public void Display()
    {
        // Display All Movies
        // loop thru Movie Lists
        for (int i = 0; i < _movieIds.Count; i++)
        {
            // display movie details
            Console.WriteLine($"Id: {_movieIds[i]}");
            Console.WriteLine($"Title: {_movieTitles[i]}");
            Console.WriteLine($"Genre(s): {_movieGenres[i]}");
            Console.WriteLine();
        }
    }
}
