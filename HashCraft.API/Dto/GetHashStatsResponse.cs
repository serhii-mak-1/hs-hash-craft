using HashCraft.Storage.DAL.Entities;
using System.Collections.Generic;

namespace HashCraft.API.Dto
{
    public class GetHashStatsResponse
    {
        public List<HashStat> Hashes { get; set; }
    }
}
