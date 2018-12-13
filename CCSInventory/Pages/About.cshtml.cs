using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCSInventory.Pages
{
    [Authorize("ReadonlyUser")]
    public class AboutModel : PageModel
    {
        public List<string> Authors = new List<string> {
            "Rolando Collantes",
            "Spencer Harston",
            "Mark Haslam",
            "Amy Lea",
            "Josh Redford",
            "Kion Shamsa",
            "Carson Smith",
            "Bryan Sutton",
            "Hannah VanderHoeven",
            "Maxwell Wright",
        };

        public void OnGet()
        {
            Shuffle(Authors);
        }

        public void Shuffle(List<string> authors)
        {
            var rand = new Random();
            int n = authors.Count;
            while (n-- > 1)
            {
                int k = rand.Next(n + 1);
                string temp = authors[k];
                authors[k] = authors[n];
                authors[n] = temp;
            }
        }
    }
}
