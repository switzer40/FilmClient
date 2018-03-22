using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmClient.Pages.Medium
{
    public class LocationListModel : PageModel
    {
        private readonly IMediumService _service;
        public LocationListModel(IMediumService service)
        {
            _service = service;
        }
        [BindProperty]
        public string LocationToList { get; set; }
        public List<string> ChosenMedia { get; set; }
        public async Task OnPostAsync()
        {
            ChosenMedia = new List<string>();
            var rawList = await _service.GetAbsolutelyAllAsync();
            foreach (KeyedMediumDto k in rawList)
            {
                if (k.Location == LocationToList)
                {
                    ChosenMedia.Add(k.Title);
                }
            }
            PrintList();
        }

        private void PrintList()
        {
            int count = ChosenMedia.Count + 2;
            string[] printList = new string[count];
            printList[0] = $"Media stored at {LocationToList}";
            printList[1] = "===================";
            for (int i = 2; i < count; i++)
            {
                printList[i] = ChosenMedia[i - 2];
            }
            var path = $"E:\\LocationLists\\{LocationToList}.txt";
            System.IO.File.WriteAllLines(path, printList);
        }
    }
}