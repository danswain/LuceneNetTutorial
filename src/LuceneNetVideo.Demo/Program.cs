using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace LuceneNetVideo.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Lucene.net");

            var fordFiesta = new Document();
            fordFiesta.Add(new Field("Id","1",Field.Store.YES,Field.Index.NOT_ANALYZED));
            fordFiesta.Add(new Field("Make","Ford",Field.Store.YES,Field.Index.ANALYZED));
            fordFiesta.Add(new Field("Model","Fiesta",Field.Store.YES,Field.Index.ANALYZED));

            var vauxhallAstra = new Document();
            vauxhallAstra.Add(new Field("Id", "2", Field.Store.YES, Field.Index.NOT_ANALYZED));
            vauxhallAstra.Add(new Field("Make", "Vauxhall", Field.Store.YES, Field.Index.ANALYZED));
            vauxhallAstra.Add(new Field("Model", "Astra", Field.Store.YES, Field.Index.ANALYZED));

            Directory directory = FSDirectory.Open(new DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndex"));

            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_29);

            var writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);

            writer.AddDocument(fordFiesta);
            writer.AddDocument(vauxhallAstra);

            writer.Optimize();
            writer.Close();

            IndexReader indexReader = IndexReader.Open(directory, true);
            Searcher indexSearch = new IndexSearcher(indexReader);

            var queryParser = new QueryParser(Version.LUCENE_29, "Make", analyzer);
            var query = queryParser.Parse("Vauxhall");

            Console.WriteLine("Searching For " + query);

            TopDocs resultDocs = indexSearch.Search(query, indexReader.MaxDoc());

            var hits = resultDocs.scoreDocs;

            foreach (var hit in hits)
            {
                var documentFromSarch = indexSearch.Doc(hit.doc);
                Console.WriteLine(documentFromSarch.Get("Make") + " " + documentFromSarch.Get("Model"));
            }



            Console.ReadLine();



        }
    }
}
