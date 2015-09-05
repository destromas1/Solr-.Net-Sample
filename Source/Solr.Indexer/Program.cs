using System;
using System.IO;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Exceptions;
using SolrNet.Impl;

namespace Solr.Indexer
{
    class Program
    {
        private static readonly string solrURL = "http://localhost:8983/solr";

        private static void Main(string[] args)
        {

            var connection = new SolrConnection(solrURL);

            Startup.Init<Product>(connection);

            
            var p = new Product
            {
                Id = "SP2514N",
                Manufacturer = "Samsung Electronics Co. Ltd.",
                Categories = new[]
                {
                    "electronics",
                    "hard drive333333333111",
                },
                Price = 92,
                InStock = true,
            };

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
            solr.Add(p);
            solr.Commit();



            p = new Product
            {
                Id = "SP2514N2",
                Manufacturer = "AAA Electronics Co. Ltd.",
                Categories = new[]
                {
                    "electronics",
                    "hard 777777777",
                },
                Price = 92,
                InStock = true,
            };

            
            solr.Add(p);
            solr.Commit();
            

            var results = solr.Query(new SolrQueryByField("id", "SP2514N"));
            
            Console.WriteLine(results[0].Price);

            

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            
            string archiveFolder = Path.Combine(baseDirectory, "docs");


            try
            {
                solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
                solr.Delete(SolrQuery.All);
                connection = (SolrConnection) ServiceLocator.Current.GetInstance<ISolrConnection>();
                foreach (var file in Directory.GetFiles(archiveFolder, "*.xml"))
                {
                    connection.Post("/update", File.ReadAllText(file, Encoding.UTF8));
                }
                solr.Commit();
                solr.BuildSpellCheckDictionary();
            }
            catch (SolrConnectionException e)
            {
                throw new Exception(string.Format("Couldn't connect to Solr. Please make sure that Solr is running on '{0}' or change the address in your web.config, then restart the application.", solrURL), e);
            }

        }

    }
}
