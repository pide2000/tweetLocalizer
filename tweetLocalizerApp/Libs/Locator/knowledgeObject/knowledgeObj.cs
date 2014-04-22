using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs
{
    abstract class knowledgeObj 
    {
        Dictionary<String, List<string>> indicatorTokens { get; set; }
        public List<Ngram> nGrams { get; set; }
        public int baseDataId { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int geoEntityId { get; set; }
        public int countryId { get; set; }
        public int admin1Id { get; set; }
        public int admin2Id { get; set; }
        public int admin3Id { get; set; }
        public int admin4Id { get; set; }

    }
}
