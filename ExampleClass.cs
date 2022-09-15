using ApplicationTemplate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTemplate
{
    internal class ExampleClass
    {
        public void ExampleMethod()
        {
            var fileService = new FileService();

            // Pass in an integer when calling the constructor
            var fileService1 = new FileService(3432);
            var fileService2 = new FileService("hello");
        }
    }
}
