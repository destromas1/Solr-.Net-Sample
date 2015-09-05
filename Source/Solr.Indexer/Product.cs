using System.Collections.Generic;
using SolrNet.Attributes;

namespace Solr.Indexer
{
    public class Product
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }

        [SolrField("manu_exact")]
        public string Manufacturer { get; set; }

        [SolrField("cat")]
        public ICollection<string> Categories { get; set; }

        [SolrField("price")]
        public decimal Price { get; set; }

        [SolrField("inStock")]
        public bool InStock { get; set; }
    }
}