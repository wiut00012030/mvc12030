using Microsoft.AspNetCore.Mvc;
using mvc12030.Models;
using Newtonsoft.Json;
using System.Text;

namespace mvc12030.Controllers
{
    public class DogsController : Controller
    {
        private readonly Uri uri = new Uri("http://ec2-51-20-123-79.eu-north-1.compute.amazonaws.com/");

        private readonly HttpClient client;

        public DogsController()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = uri;
        }

        public async Task<ActionResult> Index()
        {
            var dogs = new List<Dog>();
            var getResult = await this.client.GetAsync("api/dogs");

            if (getResult.IsSuccessStatusCode)
            {
                var response = await getResult.Content.ReadAsStringAsync();
                dogs = JsonConvert.DeserializeObject<List<Dog>>(response);
            }

            return View(dogs);
        }

        public async Task<ActionResult> Details(int id)
        {
            var dog = new Dog();
            var getResult = await this.client.GetAsync($"api/dogs/{id}");

            if (getResult.IsSuccessStatusCode)
            {
                var response = await getResult.Content.ReadAsStringAsync();
                dog = JsonConvert.DeserializeObject<Dog>(response);
            }

            return View(dog);
        }

        public ActionResult Create()
        {
            return View(new Dog());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Dog dog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var serializedDog = JsonConvert.SerializeObject(dog);
                    var content = new StringContent(serializedDog, Encoding.UTF8, "application/json");

                    var response = await this.client.PostAsync("api/dogs", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(dog);
            }
            catch (Exception)
            {
                return View(dog);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var dog = new Dog();
            var response = await this.client.GetAsync($"api/dogs/{id}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                dog = JsonConvert.DeserializeObject<Dog>(message);
            }

            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Dog dog)
        {
            try
            {
                var serializedDog = JsonConvert.SerializeObject(dog);
                var content = new StringContent(serializedDog, Encoding.UTF8, "application/json");
                var response = await this.client.PutAsync($"api/dogs/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(dog);
            }
            catch (Exception)
            {
                return View(dog);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var dog = new Dog();
            var response = await this.client.GetAsync($"api/dogs/{id}");

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                dog = JsonConvert.DeserializeObject<Dog>(message);
            }

            return View(dog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var response = await this.client.DeleteAsync($"api/dogs/{id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}
