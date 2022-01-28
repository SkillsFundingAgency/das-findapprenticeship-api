using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FAA.Data.ElasticSearch
{
    public class ElasticResponse<T>
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public Shards _shards { get; set; }
        public Hits<T> hits { get; set; }
        
        public ICollection<Hit<T>> Items => hits?.hits?.Select(h => h).ToList() ?? new List<Hit<T>>();
    }

    public class ElasticCountResponse
    {
        public int count { get; set; }
        public Shards _shards { get; set; }
    }

    public class Shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int skipped { get; set; }
        public int failed { get; set; }
    }

    public class Hits<T>
    {
        public Total total { get; set; }
        public object max_score { get; set; }
        public List<Hit<T>> hits { get; set; }
    }

    public class Total
    {
        public int value { get; set; }
        public string relation { get; set; }
    }

    public class Hit<T>
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public object _score { get; set; }
        public T _source { get; set; }
        public List<decimal> sort { get; set; }
    }
}
