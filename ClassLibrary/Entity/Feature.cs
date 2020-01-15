using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Entity
{
    [Table("Features")]
    public class Feature
    {
        [Key]
        public string FeatureId { get; set; }
        public string ProductId { get; set; }
        public string EnName { get; set; }
        public string DkName { get; set; }
        public string Value { get; set; }
        public string MeasureId { get; set; }
        public string EnMeasureValue { get; set; }
        public string DkMeasureValue { get; set; }
        public string EnDescription { get; set; }
        public string DkDescription { get; set; }

        public Feature()
        {
        }

        public Feature(string featureId, string productId, string enName, string dkName, string value, string measureId, string enMeasureValue, string dkMeasureValue, string enDescription, string dkDescription)
        {
            FeatureId = featureId ?? throw new ArgumentNullException(nameof(featureId));
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            EnName = enName ?? throw new ArgumentNullException(nameof(enName));
            DkName = dkName ?? throw new ArgumentNullException(nameof(dkName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            MeasureId = measureId ?? throw new ArgumentNullException(nameof(measureId));
            EnMeasureValue = enMeasureValue ?? throw new ArgumentNullException(nameof(enMeasureValue));
            DkMeasureValue = dkMeasureValue ?? throw new ArgumentNullException(nameof(dkMeasureValue));
            EnDescription = enDescription ?? throw new ArgumentNullException(nameof(enDescription));
            DkDescription = dkDescription ?? throw new ArgumentNullException(nameof(dkDescription));
        }
    }
}
