using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using LaborationRep.Models; 

namespace LaborationRep.Controllers
{
    public class ExhibitionsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Exhibition> exhibitionList = new List<Exhibition>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://localhost:3078/api/Exhibitions"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        exhibitionList = JsonConvert.DeserializeObject<List<Exhibition>>(apiResponse);
                    }
                }
                return View(exhibitionList);


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return RedirectToAction("Index", "Error");
            }

        }
    }
}
