using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using ClassLibrary.Entity;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using WebControllers.Helpers;

namespace Importer.Facade
{
    class CategoryFacade
    {
        //readonly CategoryController cc = new CategoryController();

        public void ReadCategoryXML()
        {

            using (new TimeMeasure("ReadCategoryXML"))
            {
                using (var _dbcontext = new DatabaseContext())
                {
                    //reads the catagory list XML and populates the Catagory_Table
                    List<Category> en_categoryList = new List<Category>();
                    List<Category> dk_categoryList = new List<Category>();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(UrlHelper.CategoryXml());

                    //Think something needs to reference Child nodes, so i may Foreach though them

                    XmlNodeList dataNodes = xmlDoc.SelectNodes("//Category");
                    foreach (XmlNode node in dataNodes)
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name.Equals("Name") && childNode.Attributes[2].Value.Equals("1"))
                            {
                                en_categoryList.Add(new Category() { CategoryId = Int32.Parse(node.Attributes[0].Value), CategoryName = childNode.Attributes[1].Value });
                            }
                            //if (childNode.Name.Equals("Name") && childNode.Attributes[2].Value.Equals("7"))
                            //{
                            //    dk_categoryList.Add(new Category(Int32.Parse(node.Attributes[0].Value), childNode.Attributes[1].Value));
                            //}
                        }
                    }


                    foreach (var item in en_categoryList)
                    {
                        Post(item);
                    }
                }
            }

        }
        public bool categoryTableIsPopulated()
        {
            using(var _dbcontext = new DatabaseContext())
            {
                if (_dbcontext.Categories.Any())
                {
                    return true;
                }
                else return false;
            }
        }


        public void Post(Category category) //Inserts Catatgories into the Catagry_table
        {
            using (new TimeMeasure("Post Catagories"))
            {

                using (var _dbcontext = new DatabaseContext())
                {
                    _dbcontext.Categories.AddOrUpdate(category);
                    _dbcontext.SaveChanges();
                    Debug.WriteLine("Added Category: " + category.CategoryId + " > " + category.CategoryName);
                }
            }
        }
    }
}
