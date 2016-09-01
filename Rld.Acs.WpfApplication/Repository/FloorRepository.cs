using System.Collections;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Repository
{
    public class FloorRepository : BaseRepository<Floor, int>, IFloorRepository
    {
        public FloorRepository()
        {
            RelevantUri = "/api/Floors";
        }

        public override bool Update(Floor floor)
        {
            return Update(floor, floor.FloorID);
        }
    }
}
