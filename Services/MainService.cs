using System;

namespace ApplicationTemplate.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{   // MainService knows Fileservice exists because in startup, we have services.AddTransient of Fileservice * we otherwise cannot instantiate an interface.
    private readonly IFileService _fileService;
    public MainService(IFileService fileService)
    {
        // we don't want to create a dependency here, as if fileService changes this code is broken
        // _fileService = new FileService();
        _fileService = fileService;
    }

    public void Invoke()
    {
        string choice;
        do
        {
            Console.WriteLine("1) Add Movie");
            Console.WriteLine("2) Display All Movies");
            Console.WriteLine("X) Quit");
            choice = Console.ReadLine();

            // Logic would need to exist to validate inputs and data prior to writing to the file
            // You would need to decide where this logic would reside.
            // Is it part of the FileService or some other service?
            if (choice == "1")
            {

                //ask the user for values to write
                _fileService.Write(99999, "My new test movie", "Action|Horror");
            }
            else if (choice == "2")
            {
                _fileService.Read();
                _fileService.Display(); 
            }
        }
        while (choice != "X");
    }
}
