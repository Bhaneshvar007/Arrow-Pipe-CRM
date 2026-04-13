using Intelligent_SKU_Match_utility.Cache;
using Intelligent_SKU_Match_utility.Core;
using Intelligent_SKU_Match_utility.Models;
using Intelligent_SKU_Match_utility.Repository;
using Intelligent_SKU_Match_utility.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intelligent_SKU_Match_utility.Utils;
using System.Threading.Tasks;
using JsonReader = Intelligent_SKU_Match_utility.Utils.JsonReader;
using System.Configuration;

namespace Intelligent_SKU_Match_utility
{
    internal class Program
    {
        static void Main(string[] args)

        {
            string sonpath = ConfigurationManager.AppSettings.Get("jsonfile");
            var inquiries = JsonReader.Load(sonpath + "aer.txt");


            MatchingEngine engine = new MatchingEngine();

            foreach (var inquiry in inquiries)
            {
                //foreach (var item in inquiry.ProcessedText)
                //{
                    string searchText = inquiry.description;

                    MatchResult result =
                    engine.FindBestMatch(
                        inquiry.InquiryId,
                        searchText);



                    
                    Console.WriteLine(
                    $"Inquiry:{result.InquiryId} SKU:{result.SKU_Id} Score:{result.Score} Type:{result.Classification}");
                //}
            }

            Console.ReadLine();
        }

        /* static void Main(string[] args)
         {
             SKURepository repo = new SKURepository();

             Console.WriteLine("Loading SKU Cache...");

             var skus = repo.GetAllSKUs();

             SKUCache.Load(skus);

             Console.WriteLine("SKU Loaded: " + skus.Count);

             InquiryService service = new InquiryService();

             while (true)
             {
                 Console.WriteLine("\nEnter Inquiry:");

                 string input = Console.ReadLine();

                 service.ProcessInquiry(input);
             }
         }*/

    }

}

