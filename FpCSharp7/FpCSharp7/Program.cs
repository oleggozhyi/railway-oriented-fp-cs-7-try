using FpCSharp7.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpCSharp7Failure
{
    static class Program
    {
        static void Main(string[] args)
        {
            var controller = new ProductController();
            controller.AddNewProduct(new AddProductRequest { Name = null })
                      .FormatHttpResult()
                      .Print();

            controller.AddNewProduct(new AddProductRequest { Name = "Samsung Galaxy Note 7" })
                      .FormatHttpResult()
                      .Print();

            controller.AddNewProduct(new AddProductRequest { Name = "Apple iPhone 7 Plus" })
                      .FormatHttpResult()
                      .Print();

            controller.AddNewProduct(new AddProductRequest { Name = "Samsung Galaxy Note 7" })
                      .FormatHttpResult()
                      .Print();
        }
        public static string FormatHttpResult(this (int statusCode, string content) httpResult)
            => "================================\n"
            + $"Code = {httpResult.statusCode} \n {httpResult.content}\n"
            + "================================\n";
        public static void Print(this string s) => Console.WriteLine(s);
    }

    
}
