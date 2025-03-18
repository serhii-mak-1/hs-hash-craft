using HashCraft.Storage.DAL.Entities;
using System.Collections.Generic;

namespace HashCraft.API.Dto
{
    public class GetHashesResponse
    {
        public List<Hash> Hashes { get; set; }
    }
}
