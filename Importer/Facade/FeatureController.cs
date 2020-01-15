using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using ClassLibrary.Entity;
using Importer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;
using WebControllers.Controllers;
using WebControllers.Models;

namespace Importer.Facade
{
    class FeatureController
    {
        readonly WebControllers.Controllers.FeatureController featureController = new WebControllers.Controllers.FeatureController();

        public Feature GetFeatureInfo(Feature feature)
        {
            using (new TimeMeasure("Get Feature Info"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                string startupPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "icecat_xml_files/FeaturesList.xml");
                xmlDoc.Load(startupPath);

                //Think something needs to reference Child nodes, so i may Foreach though them

                XmlNodeList dataNodes = xmlDoc.SelectNodes("//Feature");
                foreach (XmlNode node in dataNodes)
                {
                    if (node.Attributes[2].Value.Equals(feature.FeatureId))
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            if (childNode.Name.Equals("Descriptions"))
                            {
                                foreach (XmlNode description in childNode.ChildNodes)
                                {
                                    if (description.Attributes[1].Value.Equals("1"))
                                    {
                                        feature.EnDescription = description.InnerText;
                                    }
                                    if (description.Attributes[1].Value.Equals("7"))
                                    {
                                        feature.DkDescription = description.InnerText;
                                    }
                                }
                            }
                            if (childNode.Name.Equals("Measure"))
                            {
                                foreach (XmlNode signs in childNode.ChildNodes)
                                {
                                    foreach (XmlNode sign in signs.ChildNodes)
                                    {
                                        if (sign.Attributes[1].Value.Equals("1"))
                                        {
                                            feature.EnMeasureValue = sign.InnerText;
                                        }
                                        if (sign.Attributes[1].Value.Equals("7"))
                                        {
                                            feature.DkMeasureValue = sign.InnerText;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                return feature;
            }
        }
        public List<Feature> ReadFeatureInfoFromProduct(string productUrl)
        {
            using (new TimeMeasure("Read Feature Info From Product"))
            {
                List<Feature> featureList = new List<Feature>();
                Feature feature = new Feature();

                WebResponse response = null;
                Uri myUri = new Uri(productUrl, UriKind.Absolute);
                WebRequest request = WebRequest.Create(myUri);
                request.Credentials = new NetworkCredential("alphaslo", "KJ6j1c9y8c2YwMq8GTjc"); //Login credentials for IceCat
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.XmlResolver = null;
                settings.DtdProcessing = DtdProcessing.Parse;

                using (response = request.GetResponse())
                {
                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), settings))
                    {

                        while (reader.ReadToFollowing("Product"))
                        {
                            feature.ProductId = reader.GetAttribute("Prod_id");

                            while (reader.ReadToFollowing("ProductFeature"))
                            {
                                //resetting attribute
                                feature.Value = "";

                                feature.Value = reader.GetAttribute("Value");

                                while (reader.ReadToFollowing("Feature"))
                                {
                                    //resetting attributes
                                    feature.FeatureId = "";
                                    feature.MeasureId = "";
                                    feature.EnName = "";
                                    feature.DkName = "";
                                    feature.EnMeasureValue = "";
                                    feature.DkMeasureValue = "";
                                    feature.EnDescription = "";
                                    feature.DkDescription = "";

                                    feature.FeatureId = reader.GetAttribute("ID");

                                    reader.ReadToFollowing("Measure");
                                    feature.MeasureId = reader.GetAttribute("ID");
                                    reader.ReadToFollowing("Name");
                                    if (productUrl.Contains("/DK/"))
                                    {
                                        feature.DkName = reader.GetAttribute("Value");
                                    }
                                    else
                                    {
                                        feature.EnName = reader.GetAttribute("Value");
                                    }



                                    GetFeatureInfo(feature);
                                    featureList.Add(new Feature(feature.FeatureId, feature.ProductId, feature.EnName, feature.DkName, feature.Value, feature.MeasureId, feature.EnMeasureValue, feature.DkMeasureValue, feature.EnDescription, feature.DkDescription));

                                    //if (featureController.CheckIfFeatureExists(feature.FeatureId, feature.ProductId))
                                    //{
                                    //    Feature featureToBeUpdated = new Feature();
                                    //    featureToBeUpdated = featureController.GetFeatureByFeatureIdAndProdId(feature.FeatureId, feature.ProductId);

                                    //    if (featureToBeUpdated.EnName.Equals(""))
                                    //    {
                                    //        featureToBeUpdated.EnName = feature.EnName;
                                    //    }
                                    //    if (featureToBeUpdated.DkName.Equals(""))
                                    //    {
                                    //        featureToBeUpdated.DkName = feature.DkName;
                                    //    }
                                    //    featureController.UpdateSameFeature(featureToBeUpdated);

                                    //}
                                    //else
                                    //{
                                    //    featureList.Add(new Feature(feature.FeatureId, feature.ProductId, feature.EnName, featureDkName, feature.Value, feature.MeasureId, feature.EnMeasureValue, feature.DkMeasureValue, feature.EnDescription, feature.DkDescription));
                                    //}
                                    //break;
                                }
                            }
                        }


                    }
                    response.Close();
                    return featureList;
                }
            }

        }

        public void PostFeatures(string productUrl)
        {
            foreach (var feature in ReadFeatureInfoFromProduct(productUrl))
            {
                Debug.WriteLine("Reading Feature For Product");
                using(var _dbcontext = new DatabaseContext())
                {
                    _dbcontext.Features.AddOrUpdate(feature);
                    _dbcontext.SaveChanges();
                    Debug.WriteLine("Added Feature: " + feature.FeatureId + " > " + feature.EnName);
                }
            }
        }

        //public void PostFeature(Feature feature)
        //{
        //    using (new TimeMeasure("Post Features"))
        //    {
        //        System.Diagnostics.Debug.WriteLine("Lige før feature query..................................................");
        //        string query = "INSERT into Feature_Table(Feature_ID, Product_ID, EN_Name, DK_Name, Value, Measure_ID, EN_MeasureValue, DK_MeasureValue, EN_Description, DK_Description) VALUES (@Feature_ID, @Product_ID, @EN_Name, @DK_Name, @Value, @Measure_ID, @EN_MeasureValue, @DK_MeasureValue, @EN_Description, @DK_Description)";

        //        SqlConnection connection = new SqlConnection(connectionString);
        //        using (SqlCommand insertCommand = new SqlCommand(query, connection))
        //        {
        //            connection.Open();
        //            insertCommand.Parameters.AddWithValue("@Feature_ID", feature.featureId);
        //            insertCommand.Parameters.AddWithValue("@Product_ID", feature.productId);
        //            insertCommand.Parameters.AddWithValue("@EN_Name", feature.EnName);
        //            insertCommand.Parameters.AddWithValue("@DK_Name", feature.DkName);
        //            insertCommand.Parameters.AddWithValue("@Value", feature.value);
        //            insertCommand.Parameters.AddWithValue("@Measure_ID", feature.measureId);
        //            insertCommand.Parameters.AddWithValue("@EN_MeasureValue", feature.en_measureValue);
        //            insertCommand.Parameters.AddWithValue("@DK_MeasureValue", feature.dk_measureValue);
        //            insertCommand.Parameters.AddWithValue("@EN_Description", feature.en_description);
        //            insertCommand.Parameters.AddWithValue("@DK_Description", feature.dk_description);
        //            System.Diagnostics.Debug.WriteLine("Lige før execute query..................................................");
        //            insertCommand.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }
        //}
    }
}
