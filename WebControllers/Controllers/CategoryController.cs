using System.Data.SqlClient;
using System.Web.Http;
using WebControllers.Models;
using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System.Collections.Generic;
using System.Xml;
using System;
using WebControllers.Helpers;

namespace WebControllers.Controllers
{
    
    public class CategoryController : ApiController
    {

        // POST: api/Category
        [HttpPost]
        public void Post([FromBody] Category value, string connectionString) //Inserts Catatgories into the Catagry_table
        {
            using (new TimeMeasure("Post Catagories"))
            {


                string query = "INSERT into Category_Table(Category_ID, Category_Name) VALUES (@Category_ID, @Category_Name)";

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand insertCommand = new SqlCommand(query, connection))
                {
                    connection.Open();
                    insertCommand.Parameters.AddWithValue("@Category_ID", value.Category_ID);
                    insertCommand.Parameters.AddWithValue("@Category_Name", value.Category_Name);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        [HttpGet]
        public bool hasRows(string connectionString) //Looks through the "Catagories_Table" for content then return True or false
        {

            using (new TimeMeasure("Check if Catagories has rows"))
            {


                string query = "Select top (1) Category_ID from Category_Table";

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand selectCommand = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            connection.Close();
                            return true;
                        }
                        else
                        {
                            connection.Close();
                            return false;
                        }
                    }

                }

            }

        }

        [HttpPut]
        public void UpdateCategories(Category category, string connectionString)
        {

            using (new TimeMeasure("Post Catagories"))
            {
                Console.WriteLine("----------UPDATED---------");
                Console.WriteLine(category.Category_Name);

                string query = "Update Category_Table SET Category_Name = @Category_Name WHERE Category_ID = @Category_ID";

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand insertCommand = new SqlCommand(query, connection))
                {
                    connection.Open();
                    insertCommand.Parameters.AddWithValue("@Category_ID", category.Category_ID);
                    insertCommand.Parameters.AddWithValue("@Category_Name", category.Category_Name);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
                
               
            }
        }

        public void UpdateCategoryXML(string connectionString)

        {
            using (new TimeMeasure("ReadCategoryXML"))
            {

                //reads the catagory list XML and populates the Catagory_Table
                List<Category> en_categoryList = new List<Category>();
                List<Category> dk_categoryList = new List<Category>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("C:/inetpub/sonywebshopsolutions/icecat_xml_files/CategoriesList.xml");

                //Think something needs to reference Child nodes, so i may Foreach though them

                XmlNodeList dataNodes = xmlDoc.SelectNodes("//Category");
                foreach (XmlNode node in dataNodes)
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name.Equals("Name") && childNode.Attributes[2].Value.Equals("1"))
                        {
                            en_categoryList.Add(new Category(Int32.Parse(node.Attributes[0].Value), childNode.Attributes[1].Value));
                        }
                        if (childNode.Name.Equals("Name") && childNode.Attributes[2].Value.Equals("7"))
                        {
                            dk_categoryList.Add(new Category(Int32.Parse(node.Attributes[0].Value), childNode.Attributes[1].Value));
                        }
                    }
                }

                if (connectionString.Equals(ConnectionHelper.GetEnglishConnectionString()))
                {
                    foreach (var item in en_categoryList)
                    {
                        UpdateCategories(item, connectionString);
                    }
                }
                if (connectionString.Equals(ConnectionHelper.GetDanishConnectionString()))
                {
                    foreach (var item in dk_categoryList)
                    {
                        UpdateCategories(item, connectionString);
                    }
                }

            }

        }

    }
}
