//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rld.Acs.WpfApplication.Repository
//{
//    public class BaseRepository<TEntity, TKey>
//        where TEntity : class
//    {
//        private Uri url = new Uri("http://localhost:18827/api/values");

//        public virtual bool Insert(TEntity entity)
//        {
//            string url = "http://localhost:52824/api/register";
//            using (var http = new HttpClient())
//            {
//                //使用FormUrlEncodedContent做HttpContent
//                var content = new FormUrlEncodedContent(new Dictionary<string, string>()       
//                {    {"Id","6"},
//                     {"Name","添加zzl"},
//                     {"Info", "添加动作"}//键名必须为空
//                 });


//                var response = await http.PostAsync(url, content);

//                response.EnsureSuccessStatusCode();

//                Console.WriteLine(await response.Content.ReadAsStringAsync());
//            }



//            HttpClient client = new HttpClient();
//            client.BaseAddress = new Uri("http://localhost:1121/");
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            ErrorMessage = "";
//            bool fail = false;

//            HttpResponseMessage resp = client.PutAsync<TEntity>("api/Customers", entity, new JsonMediaTypeFormatter()).Result;

//                if (!resp.IsSuccessStatusCode)
//                {
//                    ErrorMessage += "Adding new item \n";
//                    HttpResponseMessage resp2 = client.PostAsync<Comment>("api/Comment", c, new JsonMediaTypeFormatter()).Result;
//                    if (!resp2.IsSuccessStatusCode)
//                    {
//                        ErrorMessage += "Error in updating " + c.ID.ToString() + "\n";
//                        fail = true;
//                    }
//                    else
//                    {
//                        Comment addedComment = resp2.Content.ReadAsAsync<Comment>().Result;
//                        c.ID = addedComment.ID;
//                    }
//                }

//            if (!fail)
//                ErrorMessage += "Update is successful\n";
//        }

//        public virtual bool Update(TEntity entity)
//        {
//        }

//        public virtual bool Delete(TKey key)
//        {
//        }

//        public virtual TEntity GetByKey(TKey key)
//        {
//        }

//        public virtual IEnumerable<TEntity> Query(TEntity entityCondition)
//        {
//        }
//    }
//}
