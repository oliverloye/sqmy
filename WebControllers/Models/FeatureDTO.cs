using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebControllers.Helpers;

namespace WebControllers.Models
{
    public class FeatureDTO
    {
        CustomStringCleaner cc = new CustomStringCleaner();

        //General Properties
        public int id { get; set; }
        public string featureId { get; set; }
        public string productId { get; set; }
        public string measureId { get; set; }
        public string value { get; set; }


        //English Properties
        public string en_name { get; set; }
        public string en_measureValue { get; set; }
        public string en_description { get; set; }

        //Danish Properties
        public string dk_name { get; set; }
        public string dk_measureValue { get; set; }
        public string dk_description { get; set; }

        public FeatureDTO()
        {
        }

        public FeatureDTO(string featureId, string productId, string en_name, string dk_name, string value, string measureId, string en_measureValue, string dk_measureValue, string en_description, string dk_description)
        {
            this.featureId = featureId;
            this.productId = productId;
            this.en_name = en_name;
            this.dk_name = dk_name;
            this.value = value;
            this.measureId = measureId;
            this.en_measureValue = en_measureValue;
            this.dk_measureValue = dk_measureValue;
            this.en_description = en_description;
            this.dk_description = dk_description;
        }

        public FeatureDTO(int id, string featureId, string productId, string en_name, string dk_name, string value, string measureId, string en_measureValue, string dk_measureValue, string en_description, string dk_description)
        {
            this.id = id;
            this.featureId = featureId;
            this.productId = productId;
            this.en_name = en_name;
            this.dk_name = dk_name;
            this.value = value;
            this.measureId = measureId;
            this.en_measureValue = en_measureValue;
            this.dk_measureValue = dk_measureValue;
            this.en_description = en_description;
            this.dk_description = dk_description;
        }

        public override string ToString()
        {
            return "Feature ID: " + featureId + "\n" +
                "Product ID: " + productId + "\n" +
                "EN_Name: " + en_name + "\n" +
                "DK_Name: " + dk_name + "\n" +
                "Value: " + value + "\n" +
                "Measure ID: " + measureId + "\n" +
                "En measureValue: " + en_measureValue + "\n" +
                "Dk measureValue: " + dk_measureValue + "\n" +
                "En description: " + en_description + "\n" +
                "Dk description: " + dk_description + "\n";
        }
    }
}