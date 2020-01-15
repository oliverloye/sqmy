using AlphaSolutions.Core.Diagnostics.TimeMeasures;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebControllers.Helpers;
using WebControllers.Models;

namespace WebControllers.Controllers
{
    public class FeatureController : ApiController
    {
        private string connectionString = ConnectionHelper.GetFeatureDbConnection();
        CustomQueryBuilder cb = new CustomQueryBuilder();

        // POST: Feature
        [HttpPost]
        public void PostFeature([FromBody] FeatureDTO feature)
        {
            using (new TimeMeasure("Post Features"))
            {
                System.Diagnostics.Debug.WriteLine("Lige før feature query..................................................");
                string query = "INSERT into Feature_Table(Feature_ID, Product_ID, EN_Name, DK_Name, Value, Measure_ID, EN_MeasureValue, DK_MeasureValue, EN_Description, DK_Description) VALUES (@Feature_ID, @Product_ID, @EN_Name, @DK_Name, @Value, @Measure_ID, @EN_MeasureValue, @DK_MeasureValue, @EN_Description, @DK_Description)";

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand insertCommand = new SqlCommand(query, connection))
                {
                    connection.Open();
                    insertCommand.Parameters.AddWithValue("@Feature_ID", feature.featureId);
                    insertCommand.Parameters.AddWithValue("@Product_ID", feature.productId);
                    insertCommand.Parameters.AddWithValue("@EN_Name", feature.en_name);
                    insertCommand.Parameters.AddWithValue("@DK_Name", feature.dk_name);
                    insertCommand.Parameters.AddWithValue("@Value", feature.value);
                    insertCommand.Parameters.AddWithValue("@Measure_ID", feature.measureId);
                    insertCommand.Parameters.AddWithValue("@EN_MeasureValue", feature.en_measureValue);
                    insertCommand.Parameters.AddWithValue("@DK_MeasureValue", feature.dk_measureValue);
                    insertCommand.Parameters.AddWithValue("@EN_Description", feature.en_description);
                    insertCommand.Parameters.AddWithValue("@DK_Description", feature.dk_description);
                    System.Diagnostics.Debug.WriteLine("Lige før execute query..................................................");
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }

        public bool CheckIfFeatureExists(string featureId, string productId)
        {
            string query = "SELECT * FROM Feature_Table WHERE Feature_ID = @Feature_ID AND Product_ID = @Product_ID";
            bool boolHolder = false;
            using (SqlConnection databaseConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand(query, databaseConnection))
                {
                    databaseConnection.Open();
                    selectCommand.Parameters.AddWithValue("@Feature_ID", featureId);
                    selectCommand.Parameters.AddWithValue("@Product_ID", productId);
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            boolHolder = true;
                        }
                    }
                    databaseConnection.Close();
                }
            }
            
            return boolHolder;
        }

        [HttpGet]
        public void UpdateSameFeature([FromBody]FeatureDTO feature)
        {
            using (new TimeMeasure("UpdateSameFeature"))
            {
                System.Diagnostics.Debug.WriteLine("Inde i update same Feature....................................");
                string query = "UPDATE Feature_Table SET " +
                    "Feature_ID = @Feature_ID, " +
                    "Product_ID = @Product_ID, " +
                    "EN_Name = @EN_Name, " +
                    "DK_Name = @DK_Name, " +
                    "Value = @Value, " +
                    "Measure_ID = @Measure_ID, " +
                    "EN_MeasureValue = @EN_MeasureValue, " +
                    "DK_MeasureValue = @DK_MeasureValue, " +
                    "EN_Description = @EN_Description, " +
                    "DK_Description = @DK_Description " +
                    "WHERE Feature_ID = @Feature_ID " +
                    "AND Product_ID = @Product_ID";

                SqlConnection connection = new SqlConnection(connectionString);
                using (SqlCommand insertCommand = new SqlCommand(query, connection))
                {
                    connection.Open();

                    insertCommand.Parameters.AddWithValue("@Feature_ID", feature.featureId);
                    insertCommand.Parameters.AddWithValue("@Product_ID", feature.productId);
                    insertCommand.Parameters.AddWithValue("@EN_Name", feature.en_name);
                    insertCommand.Parameters.AddWithValue("@DK_Name", feature.dk_name);
                    insertCommand.Parameters.AddWithValue("@Value", feature.value);
                    insertCommand.Parameters.AddWithValue("@Measure_ID", feature.measureId);
                    insertCommand.Parameters.AddWithValue("@EN_MeasureValue", feature.en_measureValue);
                    insertCommand.Parameters.AddWithValue("@DK_MeasureValue", feature.dk_measureValue);
                    insertCommand.Parameters.AddWithValue("@EN_Description", feature.en_description);
                    insertCommand.Parameters.AddWithValue("@DK_Description", feature.dk_description);
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        // GET: Feature By Feature ID AND Product_ID
        [HttpGet]
        public FeatureDTO GetFeatureByFeatureIdAndProdId(string featureId, string prodId)
        {
            using (new TimeMeasure("GetFeatureByFeatureIdAndProdId"))
            {
                FeatureDTO feature = new FeatureDTO();

                string query = "SELECT * FROM Feature_Table WHERE Feature_ID = '" + featureId + "' AND Product_ID = '" + prodId + "'";

                using (SqlConnection databaseConnection = new SqlConnection(connectionString))
                {
                    databaseConnection.Open();
                    using (SqlCommand selectCommand = new SqlCommand(query, databaseConnection))
                    {
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    feature.featureId = reader.GetString(1);
                                    feature.productId = reader.GetString(2);
                                    feature.en_name = reader.GetString(3);
                                    feature.dk_name = reader.GetString(4);
                                    feature.value = reader.GetString(5);
                                    feature.measureId = reader.GetString(6);
                                    feature.en_measureValue = reader.GetString(7);
                                    feature.dk_measureValue = reader.GetString(8);
                                    feature.en_description = reader.GetString(9);
                                    feature.dk_description = reader.GetString(10);

                                }
                            }
                        }
                    }
                    databaseConnection.Close();
                }
                return feature;
            }
        }

        // GET: Feature By Product_ID
        [HttpGet]
        public List<FeatureDTO> GetFeatureByProdId(string id, string connectionString) //Get a specific Product form the database searching on Prod_ID
        {
            List<FeatureDTO> features = new List<FeatureDTO>();
            using (new TimeMeasure("GetFeatureByProdId"))
            {
                string query = "SELECT * FROM Feature_Table WHERE Product_ID = '" + id + "'";

                using (SqlConnection databaseConnection = new SqlConnection(connectionString))
                {
                    databaseConnection.Open();

                    using (SqlCommand selectCommand = new SqlCommand(query, databaseConnection))
                    {
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int featureLineId = reader.GetInt32(0);
                                    string featureId = reader.GetString(1);
                                    string productId = reader.GetString(2);
                                    string en_name = reader.GetString(3);
                                    string dk_name = reader.GetString(4);
                                    string value = reader.GetString(5);
                                    string measureId = reader.GetString(6);
                                    string en_measureValue = reader.GetString(7);
                                    string dk_measureValue = reader.GetString(8);
                                    string en_description = reader.GetString(9);
                                    string dk_description = reader.GetString(10);

                                    FeatureDTO feature = new FeatureDTO(featureLineId, featureId, productId, en_name, dk_name, value, measureId, en_measureValue, dk_measureValue, en_description, dk_description);

                                    features.Add(feature);
                                }
                            }
                        }
                    }
                    databaseConnection.Close();
                }

                return features;
            }
        }
    }
}