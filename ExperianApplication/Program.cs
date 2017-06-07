// ====================================================================================================================================
// Name: Nick Velasco
// Date: 6/5/2017
// Experian Application
// Assumes data.json file is in Resource file in solution.. (can be changed or modified per location)
// Json.net/Linq  Nuget Package was added also
// ====================================================================================================================================
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExperianApplication
{
    class Program
    {
        static string PATH = @"Resources\data.json";

        /// <summary>
        /// Mains the specified arguments.
        /// 
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            try
            {
                Dictionary<ulong, PairName> pairNamesList = GetPairNamesList();
                PrintListToConsole(pairNamesList);
                System.Console.ReadLine();
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
                System.Console.ReadLine();
            }
        }

        private static Dictionary<ulong, PairName> GetPairNamesList()
        {
            Dictionary<ulong, PairName> pairNamesList = new Dictionary<ulong,PairName>();
            Recipient[] recipientList = GetRootObject().recipients;
            PairName doesItemExist;

            // Walk through the list O(n^2) traversal..
            for (int i = 0; i < recipientList.Count(); i++)
            {
                Recipient r1 = recipientList[i];

                for (int j = (i + 1); j < recipientList.Count(); j++)
                {
                   Recipient r2 = recipientList[j];
                   InsertPairNamesLogic(pairNamesList, recipientList, r1, r2);
                }
            }
            return pairNamesList;
        }

        private static void InsertPairNamesLogic(Dictionary<ulong, PairName> pairNamesList, Recipient[] recipientList, Recipient r1,Recipient r2)
        {
            PairName doesPairNamesExist; 
            var intersectingTags = r1.tags.Intersect(r2.tags).ToArray();
            bool recipientsHave2orMoreTagsInCommon = intersectingTags.Count() > 1; // readibility.. 

            if (recipientsHave2orMoreTagsInCommon)
            {
                // Take the sum of the two id's because it uniquely identifies the recipients in the feed.
                ulong currentId = (ulong)(r1.id + r2.id);
                // Check to see if added to pairNameList
                var alreadyAddedToList = pairNamesList.TryGetValue(currentId, out doesPairNamesExist);

                bool listHasPromoTag = intersectingTags.Where(x => x.ToString().Equals("promo")).Count() > 0;
                if (listHasPromoTag)
                {
                    // Check to see if promo already added
                    bool isPairPromoAdded = pairNamesList.Values.WithTag("promo").Count() > 0;
                    // (promo may only appear once in a list)
                    //  So only 1 promotion per list per first that appears in the list…
                    if (!alreadyAddedToList && !isPairPromoAdded)
                    {
                        AddPairOfNamesToList(pairNamesList, r1, r2, intersectingTags, currentId);
                    }
                }
                else
                {
                    if (!alreadyAddedToList)
                    {
                        AddPairOfNamesToList(pairNamesList, r1, r2, intersectingTags, currentId);
                    }
                }
            }
        }

        /// <summary>
        /// Avoid duplicate code
        /// Adds the pair of names to list logic.
        /// </summary>
        /// <param name="pairNamesList">The pair names list.</param>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <param name="intersectingTags">The intersecting tags.</param>
        /// <param name="currentId">The current identifier.</param>
        private static void AddPairOfNamesToList(Dictionary<ulong, PairName> pairNamesList, Recipient r1, Recipient r2, string[] intersectingTags, ulong currentId)
        {
            pairNamesList.Add(currentId, new PairName
            {
                Name1 = r1.name,
                Name2 = r2.name,
                Tags = intersectingTags
            });
        }

        static Rootobject GetRootObject()
        {
            if (!File.Exists(PATH))
            {
                // something went wrong
                return null;
            }
            using (StreamReader r = new StreamReader(PATH))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<Rootobject>(json);
            }
            return null;
        }
        
        /// <summary>
        /// Prints the list to console.
        /// </summary>
        /// <param name="pairNamesList">The pair names list.</param>
        private static void PrintListToConsole(Dictionary<ulong, PairName> pairNamesList)
        {
            foreach (var current in pairNamesList)
            {
                string tags = string.Join(" ", current.Value.Tags);
                Console.WriteLine(string.Format("Names: {0} , {1}\n{2}\n\n", current.Value.Name1, current.Value.Name2, tags));
            }
        }
      
    }

    /// <summary>
    /// An extension for PairName class to help with getting Tags
    /// </summary>
    public static class PairNameExtension
    {
        public static IEnumerable<PairName> WithTag(this IEnumerable<PairName> pairName, string tag)
        {
            if (pairName == null) return null;
            return pairName.Where(u => u.Tags.Contains(tag));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PairName
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string[] Tags { get; set; }
    
    }

    /// <summary>
    /// Json classes
    /// </summary>
    public class Rootobject
    {
    public Recipient[] recipients { get; set; }
    }

    public class Recipient
    {
    public string[] tags { get; set; }
    public string name { get; set; }
    public int id { get; set; }
    }

}
