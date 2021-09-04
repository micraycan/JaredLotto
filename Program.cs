using System;
using System.Net;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using System.Linq;

namespace JaredLotto
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonStr = new WebClient().DownloadString("https://data.ny.gov/resource/d6yy-54nr.json");
            dynamic json = JsonConvert.DeserializeObject(jsonStr);
            List<int> whiteBalls = new List<int>();
            List<int> redBalls = new List<int>();
            Dictionary<int, int> numCount = new Dictionary<int, int>();
            Dictionary<int, int> pbCount = new Dictionary<int, int>();
            List<int> mostPickedNumbers = new List<int>();

            // add numbers to list
            foreach (var o in json)
            {
                string numbers = o["winning_numbers"];
                whiteBalls.Add(int.Parse(numbers.Substring(0, 2)));
                whiteBalls.Add(int.Parse(numbers.Substring(3, 2)));
                whiteBalls.Add(int.Parse(numbers.Substring(6, 2)));
                whiteBalls.Add(int.Parse(numbers.Substring(9, 2)));
                whiteBalls.Add(int.Parse(numbers.Substring(12, 2)));
                redBalls.Add(int.Parse(numbers.Substring(15, 2)));
            }

            // count occurence of whiteballs
            foreach (int number in whiteBalls)
            {
                if (numCount.ContainsKey(number))
                {
                    numCount[number] += 1;
                } else
                {
                    numCount[number] = 1;
                }
            }

            // count occurence of redballs
            foreach (int number in redBalls)
            {
                if (pbCount.ContainsKey(number))
                {
                    pbCount[number] += 1;
                } else
                {
                    pbCount[number] = 1;
                }
            }

            var sortedWhite = from number in numCount orderby number.Value descending select number;
            var sortedRed = from number in pbCount orderby number.Value descending select number;

            for (int i = 0; i < 5; i++)
            {
                mostPickedNumbers.Add(sortedWhite.ElementAt(i).Key);
            }

            mostPickedNumbers.Sort();
            mostPickedNumbers.Add(sortedRed.ElementAt(0).Key);

            Console.WriteLine(string.Join(" ", mostPickedNumbers));

            Console.ReadLine();
        }
    }
}
