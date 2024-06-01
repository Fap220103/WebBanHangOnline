using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_Xunit
{
    public class getProductID
    {
        public List<int> getProductId()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:44353/api/")
            };
            HttpResponseMessage response = client.GetAsync("Kiemthu").Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<int>>().Result;
            }
            else return null;
        }
    }
}
